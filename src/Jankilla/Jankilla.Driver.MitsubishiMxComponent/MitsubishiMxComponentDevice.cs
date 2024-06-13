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

        public override EDriverDiscriminator Discriminator => EDriverDiscriminator.MitsubishiMxComponent;

        public override IReadOnlyList<Block> Blocks
        {
            get
            {
                return (IReadOnlyList<Block>)_blocks;
            }
        }

        #endregion

        #region Fields

        protected IList<MitsubishiMxComponentBlock> _blocks = new List<MitsubishiMxComponentBlock>();

        #endregion


        public override bool Open()
        {
            foreach (var block in _blocks)
            {
                block.Open();
            }

            IsOpened = !_blocks.Any(b => b.IsOpened == false);

            return IsOpened;
        }

        public override void Close()
        {
            foreach (var block in _blocks)
            {
                block.Close();
            }

            IsOpened = false;
        }

        public override bool ValidateBlock(Block block)
        {
            bool bValidated = ValidateContract(block);

            if (bValidated == false)
            {
                return false;
            }

            if (block.Discriminator != EDriverDiscriminator.MitsubishiMxComponent)
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

        public override bool AddBlock(Block block)
        {
            bool bValidated = ValidateBlock(block);

            if (bValidated == false)
            {
                return false;
            }

            block.Path = $"{Path}.{block.Name}";
            block.DeviceID = ID;

            _blocks.Add((MitsubishiMxComponentBlock)block);

            return true;
        }

        public override bool RemoveBlock(Block block)
        {
            block.RemoveAllTags();
            
            return _blocks.Remove((MitsubishiMxComponentBlock)block);
        }

        public override void RemoveAllBlocks()
        {
            foreach (var block in _blocks)
            {
                block.RemoveAllTags();
            }
            _blocks.Clear();
        }

        public override void ReplaceBlock(int index, Block block)
        {
            _blocks[index] = (MitsubishiMxComponentBlock)block;
        }
    }
}
