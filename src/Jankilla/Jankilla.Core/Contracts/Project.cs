using Jankilla.Core.Alarms;
using Jankilla.Core.Collections;
using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Contracts
{
    public sealed class Project : IIdentifiable
    {
        #region Public Properties

        public Guid ID { get; set; }

        public IReadOnlyList<Driver> Drivers => _drivers;
       
        public IReadOnlyList<BaseAlarm> Alarms => _alarms;

        public IReadOnlyList<Project> Remotes => _remotes;

        #endregion

        #region Fields

        private UniqueObservableCollection<Project> _remotes = new UniqueObservableCollection<Project>();

        private UniqueObservableCollection<BaseAlarm> _alarms = new UniqueObservableCollection<BaseAlarm>();
        private UniqueObservableCollection<Driver> _drivers = new UniqueObservableCollection<Driver>();

        #endregion

        #region Constructor

        public Project()
        {
            _drivers.CollectionChanged += drivers_CollectionChanged;
        }

        #endregion

        #region Public Methods
        public Driver FindDriverOrNull(Guid id)
        {
            return Drivers
                .FirstOrDefault(driver => driver.ID == id);
        }

        public Device FindDeviceOrNull(Guid id)
        {
            return Drivers
                .SelectMany(driver => driver.Devices)
                .FirstOrDefault(device => device.ID == id);
        }

        public Block FindBlockOrNull(Guid id)
        {
            return Drivers
                .SelectMany(driver => driver.Devices)
                .SelectMany(device => device.Blocks)
                .FirstOrDefault(block => block.ID == id);
        }

        public Tag FindTagOrNull(Guid id)
        {
            return Drivers
                .SelectMany(driver => driver.Devices)
                .SelectMany(device => device.Blocks)
                .SelectMany(block => block.Tags)
                .FirstOrDefault(tag => tag.ID == id);
        }

        public BaseAlarm FindAlarmOrNull(Guid id)
        {
            var stack = new Stack<BaseAlarm>(_alarms);

            while (stack.Count > 0)
            {
                var alarm = stack.Pop();

                if (alarm.ID == id)
                {
                    return alarm;
                }

                if (alarm.Discriminator == nameof(ComplexAlarm))
                {
                    var complexAlarm = (ComplexAlarm)alarm;
                    foreach (var subAlarm in complexAlarm.SubAlarms)
                    {
                        stack.Push(subAlarm);
                    }
                }
            }

            return null;
        }

        public bool AddDriver(Driver driver)
        {
            if (_drivers.Contains(driver))
            {
                return false;
            }

            _drivers.Add(driver);
            return true;
        }

        public bool RemoveDriver(Driver driver)
        {
            return _drivers.Remove(driver);
        }

        public void RemoveAllDrivers()
        {
            foreach (Driver driver in _drivers)
            {
                driver.RemoveAllDevices();
            }

            _drivers.Clear();
        }

        public bool EditDriver(Driver oldDriver, Driver newDriver)
        {
            int index = _drivers.IndexOf(oldDriver);
            if (index < 0)
            {
                return false;
            }

            _drivers[index] = newDriver;
            return true;
        }

        public bool AddAlarm(BaseAlarm alarm)
        {
            _alarms.Add(alarm);

            return true;
        }

        public bool RemoveAlarm(BaseAlarm alarm)
        {
            return _alarms.Remove(alarm);
        }

        public void RemoveAllAlarms()
        {
            _alarms.Clear();
        }

        #endregion

        #region Events

        private void drivers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (Driver driver in e.NewItems)
                        {
                            driver.Path = driver.Name;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null)
                    {

                    }
                    if (e.NewItems != null)
                    {
                        foreach (Driver driver in e.NewItems)
                        {
                            driver.Path = driver.Name;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                      
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
  
                    break;
                default:
                    break;

            }
        }

        #endregion
    }
}
