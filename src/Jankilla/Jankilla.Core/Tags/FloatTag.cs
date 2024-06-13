using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
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

        public float FloatValue { get; private set; }

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

        public FloatTag(string name, string address, EDirection inOut)
          : base(name, address, 4, inOut)
        {
            this._writebuffer = new byte[this.ByteSize];
        }

        public FloatTag(Guid id, int no, string name, int direction, int byteSize, bool readOnly,
          string address, string category, string description, string path, string unit, bool useOffset, double offset, bool useFactor, double factor, 
          int discriminator, Guid blockID)
          : this(name, address, (EDirection)direction)
        {
            ID = id;
            No = no;
            ReadOnly = readOnly;
            Description = description;
            Category = category;
            Path = path;
            Unit = unit;
            UseFactor = useFactor;
            Factor = factor;
            UseOffset = useOffset;
            Offset = offset;
            BlockID = blockID;

            Debug.Assert(byteSize == 4);
            Debug.Assert(discriminator == (int)ETagDiscriminator.Float);
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

        public override void Write(object val)
        {
            float num = (float)val;
            
            var numBytes = BitConverter.GetBytes(num);
            Buffer.BlockCopy(numBytes, 0, _writebuffer, 0, 4);

            EventHandler<TagEventArgs> writed = this.Writed;
            if (writed == null)
                return;

            writed((object)this, new TagEventArgs()
            {
                Address = this.Address,
                Buffer = this._writebuffer
            });

        }

        public override void Read(byte[] buffer, int startIndex)
        {
            throw new NotImplementedException();
        }


    }
}
