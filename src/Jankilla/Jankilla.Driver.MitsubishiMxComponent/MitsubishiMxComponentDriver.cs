using Jankilla.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMxComponent
{
    public class MitsubishiMxComponentDriver : Core.Contracts.Driver
    {
        #region Statics
        public static List<string> AllDevices = new List<string>()
        {
          "SM",
          "SB",
          "FX",
          "FY",
          "SD",
          "SW",
          "FD",
          "D",
          "B",
          "Y",
          "M",
          "L",
          "S",
          "X",
          "F",
          "V",
          "W",
          "C",
          "T",
          "R",
        };

        public static HashSet<string> BitDeviceTypes = new HashSet<string>()
        {
          "B",
          "Y",
          "M",
          "L",
          "S",
          "X",
          "F",
          "V",
          "SM",
          "SB",
          "FX",
          "FY",
        };

        public static HashSet<string> WordDeviceTypes = new HashSet<string>()
        {
          "W",
          "C",
          "D",
          "T",
          "R",
          "SD",
          "SW",
          "FD"
        };

        public static HashSet<string> HexDeviceTypes = new HashSet<string>()
        {
          "X",
          "Y",
          "B",
          "W",
          "SB",
          "SW",
          "WW",
          "WR",
          "ML",
          "MC",
          "MF"
        };

        public static HashSet<string> DecimalDeviceTypes = new HashSet<string>()
        {
          "FX",
          "FY",
          "FD",
          "SM",
          "SD",
          "M",
          "L",
          "F",
          "V",
          "D",
          "TS",
          "TC",
          "TN",
          "CS",
          "CC",
          "CN",
          "SS",
          "SC",
          "SN",
          "S",
          "A",
          "Z",
          "R",
          "ZR"
        };

        #endregion

        #region Public Properties

        public override string Discriminator => "MitsubishiMxComponent";

        public override IReadOnlyList<Device> Devices
        {
            get
            {
                return (IReadOnlyList<Device>)_devices;
            }
        }

        #endregion

        #region Fields

        protected IList<MitsubishiMxComponentDevice> _devices = new List<MitsubishiMxComponentDevice>();

        private CancellationTokenSource _cts;


        #endregion

        #region Constructor



        #endregion

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
                        Trace.WriteLine(e.Message);
                        return;
                    }
                }
            }, _cts.Token);
        }

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

        public  IReadOnlyList<Device> GetDevices()
        {
            return (IReadOnlyList<Device>)_devices;
        }

        public override bool ValidateDevice(Device device)
        {
            bool bValidated = ValidateContract(device);

            if (bValidated == false)
            {
                return false;
            }

            if (device.Discriminator != "MitsubishiMxComponent")
            {
                return false;
            }

            if (_devices.Contains(device))
            {
                return false;
            }


            return true;
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

            _devices.Add((MitsubishiMxComponentDevice)device);

            return true;
        }

        public override bool RemoveDevice(Device device)
        {
            device.RemoveAllBlocks();
            return _devices.Remove((MitsubishiMxComponentDevice)device);
        }

        public override void RemoveAllDevices()
        {
            foreach (var device in _devices)
            {
                device.RemoveAllBlocks();
            }
            _devices.Clear();
        }


    }
}
