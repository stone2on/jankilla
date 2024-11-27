using Jankilla.Core.Contracts;
using Jankilla.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.Mitsubishi.McProtocol
{
    public class MitsubishiMcProtocolDriver : Core.Contracts.Driver
    {
        public override string Discriminator => "MitsubishiMcProtocol";

        #region Fields

   
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

        public override ValidationResult AddDevice(Device device)
        {
            ValidationResult validationResult = ValidateDevice(device);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            device.Path = $"{Path}.{device.Name}";
            device.DriverID = ID;

            _devices.Add((MitsubishiMcProtocolDevice)device);

            return new ValidationResult(true, "Device added successfully.");
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


        public override ValidationResult ValidateDevice(Device device)
        {
            ValidationResult validationResult = ValidateContract(device);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (device.Discriminator != Discriminator)
            {
                return new ValidationResult(false, "Device discriminator mismatch.");
            }

            if (_devices.Contains(device))
            {
                return new ValidationResult(false, "Device already exists.");
            }

            return new ValidationResult(true, "Device validated successfully.");
        }
    }
}
