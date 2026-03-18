using Jankilla.Sample.WinForms.Controls.Pages;
using Jankilla.Sample.WinForms.Enums;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.DB;
using Jankilla.Core.Utils;
using Jankilla.Core.Tags.Base;
using Jankilla.Core.DB.SqlServer;
using Jankilla.Core.DB.SqlServer.Models;
using Jankilla.Core.Converters;
using DevExpress.XtraEditors;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace Jankilla.Sample.WinForms.Controls.Controllers
{
    public class AccessManager : IController
    {
        #region Event Handlers

        public event EventHandler<EventArgs> ProjectReloaded;
        public event EventHandler<EventArgs> PLCStatusChanged;
        public event EventHandler<EventArgs> DBStatusChanged;
        public event EventHandler<EventArgs> ProcessStatusChanged;

        #endregion

        #region Public Properties
  
        [Browsable(false)]
        public string ConnectionString
        {
            get
            {
                return $@"data source={IPAddress};initial catalog={DatabaseName};user id={ID};password={Password};MultipleActiveResultSets=True;";
            }
        }

        [Browsable(false)]
        public bool IsPLCOpened 
        {
            get 
            {
                return _isPlcOpened; 
            }
            private set
            {
                if (_isPlcOpened == value)
                {
                    return;
                }
                _isPlcOpened = value;
                PLCStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public bool IsDBOpened
        {
            get
            {
                return _isDBOpened;
            }
            private set
            {
                if (_isDBOpened == value)
                {
                    return;
                }
                _isDBOpened = value;
                DBStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public bool IsStarted
        {
            get
            {
                return _isStarted;
            }
            private set
            {
                if (_isStarted == value)
                {
                    return;
                }
                _isStarted = value;
                ProcessStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [DXCategory("DATABASE")]
        [DisplayName("User ID")]
        [ReadOnly(true)]
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                if (_settingFile != null)
                {
                    _settingFile.Write(nameof(ID), _id);

                }
            }
        }

        [DXCategory("DATABASE")]
        [DisplayName("User Password")]
        [ReadOnly(true)]
        [PasswordPropertyText(true)]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                if (_settingFile != null)
                {
                    _settingFile.Write(nameof(Password), _password);
                }
            }
        }

        [DXCategory("DATABASE")]
        [DisplayName("Database Name")]
        [ReadOnly(true)]
        public string DatabaseName
        {
            get
            {
                return _dbName;
            }
            set
            {
                _dbName = value;
                if (_settingFile != null)
                {
                    _settingFile.Write(nameof(DatabaseName), _dbName);

                }
            }
        }

        [DXCategory("DATABASE")]
        [DisplayName("IP Address")]
        [ReadOnly(true)]
        public string IPAddress

        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
                if (_settingFile != null)
                {
                    _settingFile.Write(nameof(IPAddress), _ipAddress);

                }
            }
        }

        [DXCategory("PROCESS")]
        [DisplayName("Maximum number of days to keep data")]
        public int MaximumNumberOfDays
        {
            get
            {
                return _maxNumberOfDays;
            }
            set
            {
                _maxNumberOfDays = value;
                if (_maxNumberOfDays > 90)
                {
                    _maxNumberOfDays = 90;
                }

                if (_maxNumberOfDays <= 0)
                {
                    _maxNumberOfDays = 1;
                }

                if (_settingFile != null)
                {
                    _settingFile.Write(nameof(MaximumNumberOfDays), _maxNumberOfDays.ToString());
                }
            }
        }

        [DXCategory("PROCESS")]
        [DisplayName("Auto RUN")]
        public bool AutoRun
        {
            get
            {
                return _autoRun;
            }
            set
            {
                _autoRun = value;
                if (_settingFile != null)
                {
                    _settingFile.Write(nameof(AutoRun), _autoRun.ToString());
                }
            }
        }

        [DXCategory("PROCESS")]
        [Description("Specify at what intervals(ms) you want to collect data. Please enter in milliseconds.")]
        public int Intervals
        {
            get
            {
                return _intervals;
            }
            set
            {
                _intervals = value;
                if (_intervals > 5000)
                {
                    _intervals = 5000;
                }

                if (_intervals < 500)
                {
                    _intervals = 500;
                }


                if (_settingFile != null)
                {
                    _settingFile.Write(nameof(Intervals), _intervals.ToString());
                }
            }
        }

        #endregion

        #region Fields

        private string _id;
        private string _password;
        private string _dbName;
        private string _ipAddress;

        private bool _autoRun;
        private int _intervals = 500;
        private int _maxNumberOfDays;

        private IniFile _settingFile;

        private Project _loadedProject;

        private IOrderedEnumerable<TagValueColumn> _columns = null;
        private List<Tag> _loadedTags = new List<Tag>();

        //private List<StringTag> _loadedStringTags = new List<StringTag>();
        //private List<BooleanTag> _loadedBooleanTags = new List<BooleanTag>();
        //private List<IntTag> _loadedIntTags = new List<IntTag>();
        //private List<ShortTag> _loadedShortTags = new List<ShortTag>();
        //private List<FloatTag> _loadedFloatTags = new List<FloatTag>();

        private string _localProjectFilePath;

        private bool _isPlcOpened;
        private bool _isDBOpened;
        private bool _isStarted;

        private System.Timers.Timer _collectTimer;
        private System.Timers.Timer _deleteTimer;

        #endregion

        #region Singleton

        public static AccessManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AccessManager();
                }

                return _instance;
            }
        }

        private static AccessManager _instance;
        private AccessManager()
        {
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            DirectoryInfo dir = Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}\\..\\config");

            _settingFile = new IniFile(dir.FullName);

            if (!File.Exists(_settingFile.Path))
            {
                _settingFile.Write(nameof(ID), "sa");
                _settingFile.Write(nameof(Password), "1234");
                _settingFile.Write(nameof(AutoRun), "false");
                _settingFile.Write(nameof(Intervals), "1000");
                _settingFile.Write(nameof(DatabaseName), "Jankilla.Sample.WinForms");
                _settingFile.Write(nameof(IPAddress), "127.0.0.1");
                _settingFile.Write(nameof(MaximumNumberOfDays), "30");
            }

            _id = _settingFile.Read(nameof(ID));
            _password = _settingFile.Read(nameof(Password));

            var autoRun = _settingFile.Read(nameof(AutoRun));
            bool.TryParse(autoRun, out bool bAutoRun);
            _autoRun = bAutoRun;

            var intervals = _settingFile.Read(nameof(Intervals));
            int.TryParse(intervals, out int iIntervals);
            _intervals = iIntervals;

            _dbName = _settingFile.Read(nameof(DatabaseName));
            _ipAddress = _settingFile.Read(nameof(IPAddress));

            var maxNumberOfDays = _settingFile.Read(nameof(MaximumNumberOfDays));
            int.TryParse(maxNumberOfDays, out int iMaxNumberOfDays);
            _maxNumberOfDays = iMaxNumberOfDays;

            IsDBOpened = ContractDbSet.CheckConnection(ConnectionString);
            if (IsDBOpened)
            {
                bool bExist = ContractDbSet.CheckDatabaseExists(ConnectionString, DatabaseName);
                if (bExist == false)
                {
                    ContractDbSet.CreateDatabase(ConnectionString);
                }
            }


            _collectTimer = new System.Timers.Timer();
            _collectTimer.Interval = _intervals;
            _collectTimer.Elapsed += _collectTimer_Elapsed;

            _deleteTimer = new System.Timers.Timer();
            _deleteTimer.Interval = 1000 * 60;        // 60sec
            _deleteTimer.Elapsed += _deleteTimer_Elapsed; 
        }

        public void Start()
        {
            if (_loadedProject == null)
            {
                setProject();
            }
            
            foreach (var driver in _loadedProject.Drivers)
            {
                bool bOpened = driver.Open();
                if (bOpened)
                {
                    driver.Start(Intervals);
                }
            }

            IsPLCOpened = !_loadedProject.Drivers.Any(d => d.IsOpened == false);
            IsStarted = true;

            _collectTimer.Start();

            Trace.WriteLine("Process running...");
        }

        public void Cancel()
        {
            foreach (var driver in _loadedProject.Drivers)
            {
                driver.Close();
            }

            IsStarted = false;

            _collectTimer.Stop();

            Trace.WriteLine("The process is paused.");
        }

        public IReadOnlyList<Tag> GetTags()
        {
            if (_loadedProject == null)
            {
                return null;
            }

            var tags = new List<Tag>();
            foreach (var driver in _loadedProject.Drivers)
            {
                foreach (var device in driver.Devices)
                {
                    foreach (var block in device.Blocks)
                    {
                        tags.AddRange(block.Tags);
                    }
                }
            }

            return tags;
        }

        public async Task<DataTable> GetTagValuesAsync(string category, EDateSearchOption option)
        {
            Debug.Assert(_loadedProject != null);

            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    var cols = connection
                        .Query<TagValueColumn>($"SELECT * FROM TagValueColumns WHERE ColName LIKE '{category}%'; ")
                        .ToList();

                    var colNames = cols.Select(c => c.ColName).ToList();

                    if (!cols.Any())
                    {
                        return dataTable;
                    }

                    colNames.Insert(0, "Timestamp");

                    var sb = new StringBuilder();
                    sb.Append("SELECT ");
                    foreach (var col in colNames)
                    {
                        sb.Append($"[{col}], ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    sb.Append($" FROM TagValues WHERE [Timestamp] > DATEADD(HOUR, {-(int)option}, GETDATE());");

                    var results = await connection.QueryAsync(sb.ToString());
                    if (results.Any())
                    {
                        var dataTableColMap = new Dictionary<string, string>(cols.Count());
                        dataTableColMap.Add("Timestamp", "Timestamp");

                        dataTable.Columns.Add("Timestamp", typeof(DateTime));
                        foreach (var col in cols)
                        {
                            var dtColName = col.ColName.Replace($"{category} | ", string.Empty);
                            dataTableColMap.Add(col.ColName, dtColName);

                            var tag = _loadedTags.FirstOrDefault(t => t.ID == col.ID);
                            if (tag != null)
                            {
                                Type type;
                                switch (tag.Discriminator)
                                {
                                    case ETagDiscriminator.Boolean:
                                        type = typeof(bool);
                                        break;
                                    case ETagDiscriminator.Int:
                                    case ETagDiscriminator.Short:
                                    case ETagDiscriminator.Float:
                                        type = typeof(double);
                                        break;
                                    default:
                                        type = typeof(string);
                                        break;
                                }

                                dataTable.Columns.Add(dtColName, type);
                            }
                            else
                            {
                                dataTable.Columns.Add(dtColName);
                            }
                        }
                        const int MAXIMUM = 100000;
                        foreach (var row in results.Take(MAXIMUM))
                        {
                            var dataRow = dataTable.NewRow();
                            foreach (var columnName in colNames)
                            {
                                var dtColName = dataTableColMap[columnName];
                                dataRow[dtColName] = ((IDictionary<string, object>)row)[columnName] ?? DBNull.Value;
                            }
                            dataTable.Rows.Add(dataRow);
                        }
                    }

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

            return dataTable;
        }

        public async Task LoadProjectAsync(string path)
        {
            _localProjectFilePath = path;

            await Task.Run(() =>
            {

                Project localProject = null;
                try
                {
                    localProject = JsonProjectHelper.Instance.OpenProjectFile(path);
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Failed to deserialize project file.\n {e.Message}");
                    return;
                }

                int driverCnt = 0;
                int deviceCnt = 0;
                int blockCnt = 0;
                int tagCnt = 0;
                foreach (var driver in localProject.Drivers)
                {
                    ++driverCnt;
                    Trace.WriteLine($"{driverCnt}. Driver : {driver.Name}");
                    foreach (var device in driver.Devices)
                    {
                        ++deviceCnt;
                        Trace.WriteLine($"\t{deviceCnt}. Device : {device.Name} ");
                        foreach (var block in device.Blocks)
                        {
                            ++blockCnt;
                            Trace.WriteLine($"\t\t{blockCnt}. Block : {block.Name} ");
                            foreach (var tag in block.Tags)
                            {
                                ++tagCnt;
                                Trace.WriteLine($"\t\t\t{tagCnt}. Tag : {tag.Name} ");
                            }
                        }
                    }
                }

                var fileName = Path.GetFileName(path);

                Trace.WriteLine($"{driverCnt} Drivers detected from {fileName}");
                Trace.WriteLine($"{deviceCnt} Devices detected from {fileName}");
                Trace.WriteLine($"{blockCnt} Blocks detected from {fileName}");
                Trace.WriteLine($"{tagCnt} Tags detected from {fileName}");
            });
        }

        public async Task SyncronizeProjectAsync()
        {
            if (string.IsNullOrEmpty(_localProjectFilePath))
            {
                return;
            }

            await Task.Run(() =>
            {
                Project localProject = null;
                try
                {
                    localProject = JsonProjectHelper.Instance.OpenProjectFile(_localProjectFilePath);
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Failed to deserialize project file.\n {e.Message}");
                    return;
                }

                try
                {
                    var db = new ContractDbSet(ConnectionString);
                    var dbDriverSet = db.GetAllDrivers().ToHashSet();
                    var dbDeviceSet = db.GetAllDevices().ToHashSet();
                    var dbBlockSet = db.GetAllBlocks().ToHashSet();
                    var dbTagSet = db.GetAllTags().ToHashSet();

                    foreach (var fileDriver in localProject.Drivers)
                    {
                        var idMatchedDriver = dbDriverSet.FirstOrDefault(d => d.ID == fileDriver.ID);

                        if (idMatchedDriver == null)
                        {
                            db.Add(fileDriver);
                            dbDriverSet.Remove(fileDriver);
                            Trace.WriteLine($"The driver has been added to the database. ({fileDriver.Name})");
                        }
                        else
                        {
                            if (!idMatchedDriver.Equals(fileDriver))
                            {
                                db.Update(fileDriver);
                                Trace.WriteLine($"The driver has been updated to the database. ({fileDriver.Name})");
                            }
                            dbDriverSet.Remove(idMatchedDriver);
                        }


                        foreach (var fileDevice in fileDriver.Devices)
                        {
                            var idMatchedDevice = dbDeviceSet.FirstOrDefault(d => d.ID == fileDevice.ID);

                            if (idMatchedDevice == null)
                            {
                                db.Add(fileDriver, fileDevice);
                                dbDeviceSet.Remove(fileDevice);
                                Trace.WriteLine($"\tThe device has been added to the database. ({fileDevice.Name}) ");
                            }
                            else
                            {
                                if (!idMatchedDevice.Equals(fileDevice))
                                {
                                    db.Update(fileDevice);
                                    Trace.WriteLine($"\tThe device has been updated to the database. ({fileDevice.Name}) ");
                                }
                                dbDeviceSet.Remove(idMatchedDevice);
                            }

                            foreach (var fileBlock in fileDevice.Blocks)
                            {
                                var idMatchedBlock = dbBlockSet.FirstOrDefault(d => d.ID == fileBlock.ID);

                                if (idMatchedBlock == null)
                                {
                                    db.Add(fileDevice, fileBlock);
                                    dbBlockSet.Remove(fileBlock);

                                    Trace.WriteLine($"\t\tThe block has been added to the database. ({fileBlock.Name}) ");
                                }
                                else
                                {
                                    if (!idMatchedBlock.Equals(fileBlock))
                                    {
                                        db.Update(fileBlock);
                                        Trace.WriteLine($"\t\tThe block has been updated to the database. ({fileBlock.Name}) ");
                                    }
                                    dbBlockSet.Remove(idMatchedBlock);
                                }

                                foreach (var fileTag in fileBlock.Tags)
                                {
                                    var idMatchedTag = dbTagSet.FirstOrDefault(d => d.ID == fileTag.ID);

                                    if (idMatchedTag == null)
                                    {
                                        db.Add(fileBlock, fileTag);
                                        db.AddTagValueColumn(fileTag);
                                        dbTagSet.Remove(fileTag);
                                        Trace.WriteLine($"\t\t\tThe tag has been added to the database. ({fileTag.Name}) ");
                                    }
                                    else
                                    {
                                        if (!idMatchedTag.Equals(fileTag))
                                        {
                                            db.Update(fileTag);
                                            db.UpdateTagValueColumn(fileTag);
                                            Trace.WriteLine($"\t\t\tThe tag has been updated to the database. ({fileTag.Name}) ");
                                        }
                                        dbTagSet.Remove(idMatchedTag);
                                    }

                                }
                            }
                        }
                    }

                    foreach (var tag in dbTagSet)
                    {
                        db.DeleteTagValueColumn(tag);
                        db.Delete(tag);
                    }

                    foreach (var block in dbBlockSet)
                    {
                        db.Delete(block);
                    }

                    foreach (var device in dbDeviceSet)
                    {
                        db.Delete(device);
                    }

                    foreach (var driver in dbDriverSet)
                    {
                        db.Delete(driver);
                    }

                }
                catch (Exception e)
                {
                    Trace.WriteLine($"There was a problem accessing the database and it is ending the process. Contact your administrator.\n{e.Message}");
                    return;
                }

                
                setProject();

                Trace.WriteLine("The database data has been synchronized with the local file project you specified.");
            });

            _columns = null;
        }

        #endregion

        #region Private Helpers

        private void setProject()
        {
            var db = new ContractDbSet(ConnectionString);
            try
            {
                _loadedProject = db.GetProject();

                _loadedTags.Clear();

                foreach (var driver in _loadedProject.Drivers)
                {
                    foreach (var device in driver.Devices)
                    {
                        foreach (var block in device.Blocks)
                        {
                            foreach (var tag in block.Tags)
                            {
                                _loadedTags.Add(tag);
                            }
                        }
                    }
                }

                _loadedTags = _loadedTags
                    .OrderBy(t => t.ID)
                    .ToList();

                ProjectReloaded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
                Trace.WriteLine(e.Message);
            }
        }

        private async Task collectAsync()
        {
            Debug.Assert(_loadedProject != null);

            var colSb = new StringBuilder();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
  
                    if (_columns == null)
                    {
                        _columns = connection
                            .Query<TagValueColumn>("SELECT ID, ColName FROM TagValueColumns")
                            .OrderBy(c => c.ID);
                    }
                    
                    colSb.Append("INSERT INTO TagValues (");
                    foreach (var col in _columns)
                    {
                        colSb.Append($"[{col.ColName}], ");
                    }

                    colSb.Remove(colSb.Length - 2, 2);
                    colSb.Append(") VALUES (");
                    foreach (var tag in _loadedTags)
                    {
                        colSb.Append($"'{tag.CalibratedValue}', ");
                    }
                    colSb.Remove(colSb.Length - 2, 2);
                    colSb.Append(");");

                    await connection.ExecuteAsync(colSb.ToString());

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{ex.Message} :: {colSb}");
            }
        }

        private async Task deleteAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    await connection.ExecuteAsync($"DELETE FROM TagValues WHERE [Timestamp] < DATEADD(DAY, {MaximumNumberOfDays}, GETDATE()); ");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

   

        #endregion

        #region Events

        private async void _collectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            await collectAsync();
        }

        private async void _deleteTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            await deleteAsync();
        }

        #endregion
    }
}
