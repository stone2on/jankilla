using ActUtlType64Lib;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.Mitsubishi.MxComponent
{
    public class MitsubishiMxComponentDevice : Device
    {
        #region Public Properties

        public ECpuType CpuType { get; set; }

        public override string Discriminator => nameof(MitsubishiMxComponentDevice);

        #endregion

        public override ValidationResult ValidateBlock(Block block)
        {
            ValidationResult validationResult = base.ValidateBlock(block);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var mxBlock = (MitsubishiMxComponentBlock)block;

            if (mxBlock.DeviceType == EDeviceType.Unknown)
            {
                return new ValidationResult(false, "Unknown DeviceType");
            }

            if (mxBlock.DeviceNumber == EDeviceNumber.Unknown)
            {
                return new ValidationResult(false, "Unknown DeviceNumber");
            }

            if (mxBlock.DeviceType == EDeviceType.Bit && mxBlock.StartAddressNo % 16 != 0)
            {
                return new ValidationResult(false, "Bit DeviceType not aligned to 16");
            }

            if (string.IsNullOrEmpty(mxBlock.StartAddress))
            {
                return new ValidationResult(false, "Invalid StartAddress");
            }

            if (mxBlock.StationNo < 1)
            {
                return new ValidationResult(false, "Invalid Station no.");
            }

            string deviceType = new string(mxBlock.StartAddress.TakeWhile(char.IsLetter).ToArray());

            // Validate device type exists
            if (!MitsubishiMxComponentDriver.BitDeviceTypes.Contains(deviceType) && !MitsubishiMxComponentDriver.WordDeviceTypes.Contains(deviceType))
            {
                return new ValidationResult(false, "Invalid device type");
            }

            // Word alignment check for bit devices
            if (MitsubishiMxComponentDriver.BitDeviceTypes.Contains(deviceType) && mxBlock.StartAddressNo % 16 != 0)
            {
                return new ValidationResult(false, "Bit device type not aligned to 16");
            }

            // Address range validation
            if (MitsubishiMxComponentDriver.MaxAddresses.TryGetValue((CpuType, deviceType), out int maxAddress))
            {
                int endAddress = mxBlock.StartAddressNo + (mxBlock.BufferSize * 2) - 1;
                if (endAddress > maxAddress)
                {
                    return new ValidationResult(false, "Address range exceeded");
                }
            }

            return new ValidationResult(true, "Validation successful");
        }

    
    }
}
