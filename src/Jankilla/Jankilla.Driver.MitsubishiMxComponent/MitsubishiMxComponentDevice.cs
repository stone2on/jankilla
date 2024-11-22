using ActUtlType64Lib;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMxComponent
{
    public class MitsubishiMxComponentDevice : Device
    {
        #region Public Properties

        public ECpuType CpuType { get; set; }

        public override string Discriminator => "MitsubishiMxComponent";

        #endregion

        #region Fields

  
        #endregion
  

        public override bool ValidateBlock(Block block)
        {
            bool bValidated = base.ValidateBlock(block);

            if (bValidated == false)
            {
                return false;
            }

            var mxBlock = (MitsubishiMxComponentBlock)block;


            if (mxBlock.DeviceType == EDeviceType.Unknown)
            {
                return false;
            }

            if (mxBlock.DeviceNumber == EDeviceNumber.Unknown)
            {
                return false;
            }

            if (mxBlock.DeviceType == EDeviceType.Bit && mxBlock.StartAddressNo % 16 != 0)
            {
                return false;
            }


            if (string.IsNullOrEmpty(mxBlock.StartAddress) || mxBlock.StationNo < 1)
            {
                return false;
            }

            string deviceType = new string(mxBlock.StartAddress.TakeWhile(char.IsLetter).ToArray());

            // Validate device type exists
            if (!MitsubishiMxComponentDriver.BitDeviceTypes.Contains(deviceType) && !MitsubishiMxComponentDriver.WordDeviceTypes.Contains(deviceType))
            {
                return false;
            }

            // Word alignment check for bit devices
            if (MitsubishiMxComponentDriver.BitDeviceTypes.Contains(deviceType) && mxBlock.StartAddressNo % 16 != 0)
            {
                return false;
            }

            // Address range validation
            if (MitsubishiMxComponentDriver.MaxAddresses.TryGetValue((CpuType, deviceType), out int maxAddress))
            {
                int endAddress = mxBlock.StartAddressNo + (mxBlock.BufferSize * 2) - 1;
                if (endAddress > maxAddress)
                {
                    return false;
                }
            }

            return true;
        }

    
    }
}
