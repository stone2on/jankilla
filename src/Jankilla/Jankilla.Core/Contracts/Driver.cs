using Jankilla.Core.Collections;
using Jankilla.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Core.Contracts
{
    public abstract class Driver : BaseContract, IDisposable
    {
        #region Public Properties

        public IReadOnlyList<Device> Devices => _devices;
      
        #endregion

        #region Fields

        protected UniqueObservableCollection<Device> _devices = new UniqueObservableCollection<Device>();
        protected CancellationTokenSource _cts;
        private bool _disposedValue;

        #endregion

        #region Constructor

        protected Driver()
        {
            _devices.CollectionChanged += devices_CollectionChanged;
        }

        protected virtual void devices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (Device device in e.NewItems)
                        {
                            device.Path = $"{Path}.{device.Name}";
                            device.DriverID = ID;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null)
                    {
            
                    }
                    if (e.NewItems != null)
                    {
                        foreach (Device device in e.NewItems)
                        {
                            device.Path = $"{Path}.{device.Name}";
                            device.DriverID = ID;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (Device device in e.OldItems)
                        {
                            device.RemoveAllBlocks();
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    RemoveAllDevices();
                    break;
                default:
                    break;

            }
        }

        #endregion

        #region Public Methods

        public virtual ValidationResult ValidateDevice(Device device)
        {
            ValidationResult validationResult = ValidateContract(device);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (_devices.Contains(device))
            {
                return new ValidationResult(false, "Device already exists in the collection.");
            }

            return new ValidationResult(true, "Device is valid.");
        }

        public virtual ValidationResult AddDevice(Device device)
        {
            ValidationResult validationResult = ValidateDevice(device);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            device.Path = $"{Path}.{device.Name}";
            device.DriverID = ID;

            _devices.Add(device);

            return new ValidationResult(true, "Device added successfully.");
        }

        public virtual bool RemoveDevice(Device device)
        {
            device.RemoveAllBlocks();
            return _devices.Remove(device);
        }

        public virtual void RemoveAllDevices()
        {
            foreach (var device in _devices)
            {
                device.RemoveAllBlocks();
            }
            _devices.Clear();
        }

        public virtual void Start(int tick)
        {
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        _cts.Token.ThrowIfCancellationRequested();
                        foreach (var device in Devices)
                        {
                            var blocks = device.Blocks;

                            foreach (var block in blocks)
                            {
                                block.SuppressTagEvents(true);
                            }

                            foreach (var block in blocks)
                            {
                                block.Read();
                            }

                            foreach (var block in blocks)
                            {
                                block.SuppressTagEvents(false);
                            }

                            foreach (var block in blocks)
                            {
                                block.Write();
                            }
                        }

                        Thread.Sleep(tick);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                        return;
                    }
                }
            }, _cts.Token);
        }

        public override bool Open()
        {
            if (Devices == null || Devices.Count == 0)
            {
                return false;
            }

            foreach (var device in Devices)
            {
                device.Open();
            }

            IsOpened = !Devices.Any(b => b.IsOpened == false);

            return true;
        }

        public override void Close()
        {
            _cts?.Cancel();

            foreach (var device in Devices)
            {
                device.Close();
            }

            IsOpened = false;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"{Name} / {Discriminator}";
        }

        public override bool Equals(object obj)
        {
            return obj is Driver driver &&
                   ID.Equals(driver.ID) &&
                   Name == driver.Name &&
                   Path == driver.Path &&
                   Description == driver.Description &&
                   Discriminator == driver.Discriminator;
        }

        public override int GetHashCode()
        {
            int hashCode = 2040496800;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + Discriminator.GetHashCode();
            return hashCode;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _devices.CollectionChanged -= devices_CollectionChanged;
                }

                _disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }



        #endregion

    }
}
