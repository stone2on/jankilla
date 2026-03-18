using Jankilla.Core.Contracts;
using Jankilla.Core.Utils;
using Jankilla.Driver.Mitsubishi.McProtocol.Defines;
using Jankilla.Driver.Mitsubishi.McProtocol.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Driver.Mitsubishi.McProtocol
{
    public class MitsubishiMcProtocolDevice : Device
    {
        #region Public Properties

        public override string Discriminator => "MitsubishiMcProtocol";
      
        public EProtocol Protocol => EProtocol.TCP;

        public string IPAddress { get; set; }
        public int Port { get; set; }
        public EFrame Frame { get; set; }

        #endregion

        #region Fields
    
        private McProtocolTcp _protocol;

        #endregion

        public override bool Open()
        {
            _protocol?.Close();
            _protocol = new McProtocolTcp(IPAddress, Port, Frame);

            foreach (var block in _blocks)
            {
                block.Open();
                //block.DeviceProtocol = _protocol;
            }

            IsOpened = !_blocks.Any(b => b.IsOpened == false);

            return IsOpened;
        }

        public override void Close()
        {
            _protocol?.Close();

            foreach (var block in _blocks)
            {
                block.Close();
            }

            IsOpened = false;
        }


        public override ValidationResult ValidateBlock(Block block)
        {
            ValidationResult validationResult = ValidateContract(block);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (block.Discriminator != "Mitsubishi.McProtocol")
            {
                return new ValidationResult(false, "Invalid block discriminator.");
            }

            if (_blocks.Contains(block))
            {
                return new ValidationResult(false, "Block already exists.");
            }

            if (string.IsNullOrEmpty(block.StartAddress))
            {
                return new ValidationResult(false, "Start address is null or empty.");
            }

            var mxBlock = (MitsubishiMcProtocolBlock)block;

            if (string.IsNullOrEmpty(mxBlock.StartAddress))
            {
                return new ValidationResult(false, "MX block start address is null or empty.");
            }

            if (mxBlock.DeviceType == EDeviceType.Unknown)
            {
                return new ValidationResult(false, "Unknown device type.");
            }

            if (mxBlock.DeviceNumber == EDeviceNumber.Unknown)
            {
                return new ValidationResult(false, "Unknown device number.");
            }

            //if (mxBlock.DeviceType == EDeviceType.Bit && mxBlock.StartAddressNo % 16 != 0)
            //{
            //    return new ValidationResult(false, "Invalid start address number for bit device type.");
            //}

            return new ValidationResult(true, "Block is valid.");
        }
    }
}
