using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Jankilla.Driver.MitsubishiMxComponent
{
    public class MitsubishiMxComponentTagWrapper
    {
        public int BufferStartIndex { get; set; }

        public Tag Tag { get; set; }

        public MitsubishiMxComponentTagWrapper(Tag tag, int bufferStartIndex, string deviceCode, EDeviceNumber numberType)
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

        public override bool Equals(object obj)
        {
            return obj is MitsubishiMxComponentTagWrapper wrapper &&
                   EqualityComparer<Tag>.Default.Equals(Tag, wrapper.Tag);
        }

        public override int GetHashCode()
        {
            return 1005349675 + EqualityComparer<Tag>.Default.GetHashCode(Tag);
        }
    }
}
