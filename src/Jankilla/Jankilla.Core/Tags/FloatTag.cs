using Jankilla.Core.Alarms;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Contracts.Tags
{
    public class FloatTag : Tag
    {
        public override event EventHandler<TagEventArgs> Writed;
        public override event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public float FloatValue { get; private set; }
        [JsonIgnore]
        public override object Value
        {
            get => (object)this.FloatValue;
            protected set
            {
                this.FloatValue = (float)value;

                double calibratedVal = FloatValue;
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

        private double _calibratedVal;
        [JsonIgnore]
        public override object CalibratedValue 
        {
            get => _calibratedVal;
            protected set 
            {
                _calibratedVal = (double)value;
                this.NotifyPropertyChanged(this.PropertyChanged, nameof(CalibratedValue));
            }
        }

 
        public override ETagDiscriminator Discriminator => ETagDiscriminator.Float;

        public FloatTag()
        {
            ByteSize = 4;
        }

        public override void Read(short[] buffer, int startIndex)
        {
            if (CompareByteArrayToShortArray(_readbuffer, 0, buffer, startIndex, _readbuffer.Length))
            {
                return;
            }

            this.Copy(buffer, startIndex);
            this.Value = BitConverter.ToSingle(_readbuffer, 0);
        }

        public override void Read(byte[] buffer, int startIndex)
        {
            if (CompareByteArrays(_readbuffer, 0, buffer, startIndex, _readbuffer.Length))
            {
                return;
            }

            this.Copy(buffer, startIndex);
            this.Value = BitConverter.ToSingle(this._readbuffer, 0);
        }

        public override void Write(object val)
        {
            float num = (float)val;
            
            var numBytes = BitConverter.GetBytes(num);
            Buffer.BlockCopy(numBytes, 0, _writebuffer, 0, 4);

            EventHandler<TagEventArgs> writed = this.Writed;
            if (writed == null)
                return;

            writed(this, new TagEventArgs()
            {
                Address = this.Address,
                Buffer = this._writebuffer
            });

        }

    }
}
