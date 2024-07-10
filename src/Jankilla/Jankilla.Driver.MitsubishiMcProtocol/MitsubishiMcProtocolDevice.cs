using Jankilla.Core.Contracts;
using Jankilla.Driver.MitsubishiMcProtocol.Defines;
using Jankilla.Driver.MitsubishiMcProtocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMcProtocol
{
    public class MitsubishiMcProtocolDevice : Device
    {
        #region Public Properties

        public override string Discriminator => "MitsubishiMcProtocol";
        public override IReadOnlyList<Block> Blocks => (IReadOnlyList<Block>)_blocks;
        public EProtocol Protocol => EProtocol.TCP;

        public string IPAddress { get; set; }
        public int Port { get; set; }
        public EFrame Frame { get; set; }

        #endregion

        #region Fields

        protected IList<MitsubishiMcProtocolBlock> _blocks = new List<MitsubishiMcProtocolBlock>();

        private McProtocolTcp _protocol;

        #endregion

        public override bool Open()
        {
            _protocol?.Close();
            _protocol = new McProtocolTcp(IPAddress, Port, Frame);

            foreach (var block in _blocks)
            {
                block.Open();
                block.DeviceProtocol = _protocol;
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

        public override bool AddBlock(Block block)
        {
            bool bValidated = ValidateBlock(block);

            if (bValidated == false)
            {
                return false;
            }

            block.Path = $"{Path}.{block.Name}";
            block.DeviceID = ID;

            _blocks.Add((MitsubishiMcProtocolBlock)block);

            return true;
        }

        public override void RemoveAllBlocks()
        {
            foreach (var block in _blocks)
            {
                block.RemoveAllTags();
            }
            _blocks.Clear();
        }

        public override bool RemoveBlock(Block block)
        {
            block.RemoveAllTags();

            return _blocks.Remove((MitsubishiMcProtocolBlock)block);
        }

        public override void ReplaceBlock(int index, Block block)
        {
            _blocks[index] = (MitsubishiMcProtocolBlock)block;
        }

        public override bool ValidateBlock(Block block)
        {
            bool bValidated = ValidateContract(block);

            if (bValidated == false)
            {
                return false;
            }

            if (block.Discriminator != "MitsubishiMcProtocol")
            {
                return false;
            }

            if (_blocks.Contains(block))
            {
                return false;
            }

            if (string.IsNullOrEmpty(block.StartAddress))
            {
                return false;
            }

            var mxBlock = (MitsubishiMcProtocolBlock)block;

            if (string.IsNullOrEmpty(mxBlock.StartAddress))
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

            //if (mxBlock.DeviceType == EDeviceType.Bit && mxBlock.StartAddressNo % 16 != 0)
            //{
            //    return false;
            //}

            return true;
        }
    }
}
