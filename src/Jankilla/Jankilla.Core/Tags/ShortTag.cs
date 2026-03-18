using Jankilla.Core.Alarms;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Jankilla.Core.Tags
{
    public class ShortTag : Tag
    {
        public override event EventHandler<TagEventArgs> Writed;
        public override event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public short ShortValue { get; private set; }
        [JsonIgnore]
        public override object Value
        {
            get => ShortValue;
            protected set
            {
                this.ShortValue = (short)value;
                double calibratedVal = ShortValue;
                if (UseFactor)
                {
                    calibratedVal *= Factor;
                }

                if (UseOffset)
                {
                    calibratedVal += Offset;
                }

                CalibratedValue = calibratedVal;
                this.NotifyPropertyChanged(this.PropertyChanged, nameof(Value));
            }
        }
        [JsonIgnore]
        private double _calibratedVal;
        public override object CalibratedValue
        {
            get => _calibratedVal;
            protected set
            {
                _calibratedVal = (double)value;
                this.NotifyPropertyChanged(this.PropertyChanged, nameof(CalibratedValue));
            }
        }

        public override ETagDiscriminator Discriminator => ETagDiscriminator.Short;

        public ShortTag()
        {
            ByteSize = 2;
        }

        public override void Read(short[] buffer, int startIndex)
        {
            if (CompareByteArrayToShortArray(_readbuffer, 0, buffer, startIndex, _readbuffer.Length))
            {
                return;
            }

            this.Copy(buffer, startIndex);
            this.Value = buffer[startIndex];
        }

        public override void Read(byte[] buffer, int startIndex)
        {
            if (CompareByteArrays(_readbuffer, 0, buffer, startIndex, _readbuffer.Length))
            {
                return;
            }

            this.Copy(buffer, startIndex);
            this.Value = buffer[startIndex];
        }


        public override void Write(object val)
        {
            short num = (short)val;
            this._writebuffer[1] = (byte)((uint)num >> 8);
            this._writebuffer[0] = (byte)num;
            if (this.Writed == null)
            {
                return;
            }

            Writed(this, new TagEventArgs()
            {
                Address = this.Address,
                Buffer = this._writebuffer
            });

        }


       
    }
}
