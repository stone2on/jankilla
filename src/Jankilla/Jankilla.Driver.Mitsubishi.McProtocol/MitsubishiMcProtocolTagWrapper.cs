using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Tags;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Driver.Mitsubishi.McProtocol
{
    public class MitsubishiMcProtocolTagWrapper
    {
        public int BufferStartIndex { get; set; }

        public Tag Tag { get; set; }

        public MitsubishiMcProtocolTagWrapper(Tag tag, int bufferStartIndex, string deviceCode, EDeviceNumber numberType)
        {
            this.Tag = tag;
            this.BufferStartIndex = bufferStartIndex;

            if (tag.Discriminator == Core.Tags.Base.ETagDiscriminator.Boolean)
            {
                var bTag = (BooleanTag)tag;

                string type = bTag.Address.Substring(0, deviceCode.Length);
                string addr = bTag.Address.Substring(deviceCode.Length);

                int num = numberType != EDeviceNumber.Hex ? int.Parse(addr) : int.Parse(addr, NumberStyles.HexNumber);

                bTag.SetModifiedAddress($"{type}{num - bTag.BitIndex}");
            }

        }
    }
}
