using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Tags
{
    public class DoubleTag : Tag
    {
        public override event EventHandler<TagEventArgs> Writed;
        public override event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public double DoubleValue { get; private set; }

        [JsonIgnore]
        public override object Value
        {
            get => DoubleValue;
            protected set
            {
                this.DoubleValue = (double)value;
                double calibratedVal = DoubleValue;
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

        public override ETagDiscriminator Discriminator => ETagDiscriminator.Double;

        public DoubleTag()
        {
            ByteSize = 8;
        }

        public override void Read(short[] buffer, int startIndex)
        {
            if (CompareByteArrayToShortArray(_readbuffer, 0, buffer, startIndex, _readbuffer.Length))
            {
                return;
            }
            this.Copy(buffer, startIndex);
            byte[] byteBuffer = new byte[8];
            Buffer.BlockCopy(buffer, startIndex * 2, byteBuffer, 0, 8);
            this.Value = BitConverter.ToDouble(byteBuffer, 0);
        }

        public override void Read(byte[] buffer, int startIndex)
        {
            if (CompareByteArrays(_readbuffer, 0, buffer, startIndex, _readbuffer.Length))
            {
                return;
            }
            this.Copy(buffer, startIndex);
            this.Value = BitConverter.ToDouble(buffer, startIndex);
        }

        public override void Write(object val)
        {
            double num = (double)val;
            byte[] bytes = BitConverter.GetBytes(num);
            Buffer.BlockCopy(bytes, 0, _writebuffer, 0, 8);

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
