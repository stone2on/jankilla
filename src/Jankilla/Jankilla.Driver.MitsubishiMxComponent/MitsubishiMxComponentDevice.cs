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

            if (string.IsNullOrEmpty(mxBlock.StartAddress))
            {
                return false;
            }

            if (mxBlock.StationNo < 1)
            {
                return false;
            }

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

            return true;
        }

    
    }
}
