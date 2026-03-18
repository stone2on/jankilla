using Dapper;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.DB.SqlServer.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.SqlServer
{
    public class ContractDbSet
    {
        public string ConnectionString { get; set; }

        private DriverRepository _driverRepository;
        private DeviceRepository _deviceRepository;
        private BlockRepository _blockRepository;
        private TagRepository _tagRepository;

        public ContractDbSet(string connStr)
        {
            ConnectionString = connStr;

            _driverRepository = new DriverRepository(connStr);
            _deviceRepository = new DeviceRepository(connStr);
            _blockRepository = new BlockRepository(connStr);
            _tagRepository = new TagRepository(connStr);
        }


        public static bool CheckConnection(string connStr)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                try
                {
                    connection.Open();
                    Trace.WriteLine("DB Connection check : [OK]");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    return false;
                }
            }

            return true;
        }
        public static bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                sqlCreateDBQuery = $"SELECT database_id FROM sys.databases WHERE Name = '{databaseName}'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, connection))
                    {
                        connection.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        connection.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Debug.Assert(false, ex.Message);
                Trace.WriteLine(ex.Message);
            }

            return result;
        }
        public static void CreateDatabase(string connectionString)
        {
            // 데이터베이스 이름을 connectionString에서 추출
            var builder = new SqlConnectionStringBuilder(connectionString);
            string databaseName = builder.InitialCatalog;

            string script = $@"
USE [master]
GO
CREATE DATABASE [{databaseName}]
GO
ALTER DATABASE [{databaseName}] SET COMPATIBILITY_LEVEL = 120
GO
ALTER DATABASE [{databaseName}] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [{databaseName}] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [{databaseName}] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [{databaseName}] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [{databaseName}] SET ARITHABORT OFF 
GO
ALTER DATABASE [{databaseName}] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [{databaseName}] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [{databaseName}] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [{databaseName}] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [{databaseName}] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [{databaseName}] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [{databaseName}] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [{databaseName}] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [{databaseName}] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [{databaseName}] SET  DISABLE_BROKER 
GO
ALTER DATABASE [{databaseName}] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [{databaseName}] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [{databaseName}] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [{databaseName}] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [{databaseName}] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [{databaseName}] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [{databaseName}] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [{databaseName}] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [{databaseName}] SET  MULTI_USER 
GO
ALTER DATABASE [{databaseName}] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [{databaseName}] SET DB_CHAINING OFF 
GO
ALTER DATABASE [{databaseName}] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [{databaseName}] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [{databaseName}] SET DELAYED_DURABILITY = DISABLED 
GO
USE [{databaseName}]
GO
CREATE TABLE [dbo].[Blocks](
    [ID] [uniqueidentifier] NOT NULL,
    [Discriminator] [int] NULL,
    [BufferSize] [int] NULL,
    [DeviceID] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[BooleanTags](
    [ID] [uniqueidentifier] NOT NULL,
    [BitIndex] [int] NULL,
PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Devices](
    [ID] [uniqueidentifier] NOT NULL,
    [Discriminator] [int] NULL,
    [DriverID] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Drivers](
    [ID] [uniqueidentifier] NOT NULL,
    [Discriminator] [int] NULL,
PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[MitsubishiMxComponentBlocks](
    [ID] [uniqueidentifier] NOT NULL,
    [StationNo] [int] NULL,
PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[MitsubishiMxComponentDevices](
    [ID] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW PAGE LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[MitsubishiMxComponentDrivers](
    [ID] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW PAGE LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
";

            try
            {
                string[] commands = script.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var command in commands)
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                        {
                            sqlCommand.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }

                Trace.WriteLine("Database and tables created successfully.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("An error occurred: " + ex.Message);
            }
        }



        public static double GetDatabaseSize(string connectionString, string databaseName)
        {
            string query = $"SELECT CAST(SUM(size) * 1.0 / 128 AS float) FROM sys.master_files WHERE database_id = DB_ID('{databaseName}')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                    return Convert.ToDouble(result);
                else
                    return 0;
            }
        }

        public static bool BackupDatabase(string connectionString, string databaseName, string backupPath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string backupCommand = $"BACKUP DATABASE [{databaseName}] TO DISK='{backupPath}\\{databaseName}_backup.bak'";
                    SqlCommand command = new SqlCommand(backupCommand, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error while backing up database: {ex.Message}");
                return false;
            }
        }

        public void AddTagValueColumn(Tag tag)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string colName = $"{tag.Category} | {tag.Name}";
                    int duplicates = connection.QueryFirst<int>($"SELECT COUNT(*) FROM TagValueColumns WHERE ColName = '{colName}'");
                    if (duplicates > 0)
                    {
                        colName = $"{colName}_{duplicates}";
                    }

                    string dataType = null;
                    switch (tag.Discriminator)
                    {
                        case Tags.Base.ETagDiscriminator.Boolean:
                            dataType = "BIT";
                            break;
                        case Tags.Base.ETagDiscriminator.Int:
                        case Tags.Base.ETagDiscriminator.Short:
                        case Tags.Base.ETagDiscriminator.UShort:
                        case Tags.Base.ETagDiscriminator.Float:
                            dataType = "FLOAT";
                            break;
                        default:
                            dataType = "NVARCHAR(50)";
                            break;
                    }

                    connection.Execute($@"INSERT INTO TagValueColumns (ID,ColName) VALUES ('{tag.ID}','{colName}');");
                    connection.Execute($"ALTER TABLE TagValues ADD [{colName}] {dataType};");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error while tag values columns search on database: {ex.Message}");
            }

        }

        public void UpdateTagValueColumn(Tag tag)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string oldColName = connection.QueryFirst<string>($"SELECT ColName FROM TagValueColumns WHERE ID = '{tag.ID}'");

                    string colName = $"{tag.Category} | {tag.Name}";

                    if (oldColName == colName)
                    {
                        return;
                    }

                    int duplicates = connection.QueryFirst<int>($"SELECT COUNT(*) FROM TagValueColumns WHERE ColName = '{colName}'");
                    if (duplicates > 0)
                    {
                        colName = $"{colName}_{duplicates}";
                    }

                    connection.Execute($"UPDATE TagValueColumns SET ColName = '{colName}' WHERE ID = '{tag.ID}';");
                    connection.Execute($"EXEC sp_rename '[dbo].[TagValues].[{oldColName}]', '{colName}', 'COLUMN'; ");

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error while tag values columns search on database: {ex.Message}");
            }

        }

        public void DeleteTagValueColumn(Tag tag)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string oldColName = connection.QueryFirst<string>($"SELECT ColName FROM TagValueColumns WHERE ID = '{tag.ID}'");

                    connection.Execute("DELETE FROM TagValueColumns WHERE ID = @ID;", tag);

                    connection.Execute($"ALTER TABLE TagValues DROP COLUMN [{oldColName}];");

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error while tag values columns search on database: {ex.Message}");
            }

        }



        public Project GetProject()
        {
            var project = new Project();

            var dbDrivers = _driverRepository.GetAll();
            var dbDevices = _deviceRepository.GetAll();
            var dbBlocks = _blockRepository.GetAll();
            var dbTags = _tagRepository.GetAll();

            foreach (var driver in dbDrivers)
            {
                project.AddDriver(driver);
                var devices = dbDevices.Where(d => d.DriverID == driver.ID);
                foreach (var device in devices)
                {
                    driver.AddDevice(device);
                    var blocks = dbBlocks.Where(b => b.DeviceID == device.ID);
                    foreach (var block in blocks)
                    {
                        device.AddBlock(block);
                        var tags = dbTags.Where(t => t.BlockID == block.ID);
                        foreach (var tag in tags)
                        {
                            block.AddTag(tag);
                        }
                    }
                }
            }

            return project;
        }

        public int Add(Core.Contracts.Driver driver)
        {
            return _driverRepository.Add(driver);
        }

        public int Add(Contracts.Driver parent, Device device)
        {
            return _deviceRepository.Add(parent, device);
        }

        public int Add(Contracts.Device parent, Block block)
        {
            return _blockRepository.Add(parent, block);
        }

        public int Add(Block parent, Tag tag)
        {
            return _tagRepository.Add(parent, tag);
        }

        public int Delete(Core.Contracts.Driver driver)
        {
            return _driverRepository.Delete(driver);
        }

        public int Delete(Device device)
        {
            return _deviceRepository.Delete(device);
        }

        public int Delete(Block block)
        {
            return _blockRepository.Delete(block);
        }

        public int Delete(Tag tag)
        {
            return _tagRepository.Delete(tag);
        }

        public IEnumerable<Core.Contracts.Driver> GetAllDrivers()
        {
            return _driverRepository.GetAll();
        }

        public IEnumerable<Device> GetAllDevices()
        {
            return _deviceRepository.GetAll();
        }

        public IEnumerable<Device> GetAllDevices(Contracts.Driver parent)
        {
            return _deviceRepository.GetAll(parent);
        }

        public IEnumerable<Block> GetAllBlocks()
        {
            return _blockRepository.GetAll();
        }

        public IEnumerable<Block> GetAllBlocks(Device parent)
        {
            return _blockRepository.GetAll(parent);
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _tagRepository.GetAll();
        }

        public IEnumerable<Tag> GetAllTags(Block parent)
        {
            return _tagRepository.GetAll(parent);
        }

        public int Update(Core.Contracts.Driver driver)
        {
            return _driverRepository.Update(driver);
        }

        public int Update(Device device)
        {
            return _deviceRepository.Update(device);
        }

        public int Update(Block block)
        {
            return _blockRepository.Update(block);
        }

        public int Update(Tag tag)
        {
            return _tagRepository.Update(tag);
        }
    }
}
