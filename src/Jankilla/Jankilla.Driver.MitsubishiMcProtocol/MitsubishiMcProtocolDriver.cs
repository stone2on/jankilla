using Jankilla.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMcProtocol
{
    public class MitsubishiMcProtocolDriver : Core.Contracts.Driver
    {
        public override string Discriminator => "MitsubishiMcProtocol";

        public override IReadOnlyList<Device> Devices
        {
            get
            {
                return (IReadOnlyList<Device>)_devices;
            }
        }

        #region Fields

        protected IList<MitsubishiMcProtocolDevice> _devices = new List<MitsubishiMcProtocolDevice>();

        private CancellationTokenSource _cts;

        #endregion

        public override bool Open()
        {
            foreach (var device in _devices)
            {
                device.Open();
            }

            IsOpened = !_devices.Any(b => b.IsOpened == false);

            return true;
        }

        public override void Close()
        {
            _cts?.Cancel();

            foreach (var device in _devices)
            {
                device.Close();
            }

            IsOpened = false;
        }

        public override bool AddDevice(Device device)
        {
            bool bValidated = ValidateDevice(device);

            if (bValidated == false)
            {
                return false;
            }

            device.Path = $"{Path}.{device.Name}";
            device.DriverID = ID;

            _devices.Add((MitsubishiMcProtocolDevice)device);

            return true;
        }

        public override void RemoveAllDevices()
        {
            foreach (var device in _devices)
            {
                device.RemoveAllBlocks();
            }
            _devices.Clear();
        }

        public override bool RemoveDevice(Device device)
        {
            device.RemoveAllBlocks();
            return _devices.Remove((MitsubishiMcProtocolDevice)device);
        }

        public override void Start(int tick)
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
                        foreach (var device in _devices)
                        {
                            var blocks = device.Blocks;

                            foreach (var block in blocks)
                            {
                                block.Read();
                                block.Write();
                            }
                        }

                        Thread.Sleep(tick);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        return;
                    }
                }
            }, _cts.Token);
        }

        public override bool ValidateDevice(Device device)
        {
            bool bValidated = ValidateContract(device);

            if (bValidated == false)
            {
                return false;
            }

            if (device.Discriminator != Discriminator)
            {
                return false;
            }

            if (_devices.Contains(device))
            {
                return false;
            }

            return true;
        }
    }
}
