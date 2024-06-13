using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Jankilla.Core.Contracts.Tags
{
    public class ShortTag : Tag
    {
        public override event EventHandler<TagEventArgs> Writed;

        public override event PropertyChangedEventHandler PropertyChanged;

        public short ShortValue { get; private set; }

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

        public ShortTag(string name, string address, EDirection inOut)
          : base(name, address, 2, inOut)
        {
            this._writebuffer = new byte[this.ByteSize];
        }

        public ShortTag(Guid id, int no, string name, int direction, int byteSize, bool readOnly,
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

            Debug.Assert(byteSize == 2);
            Debug.Assert(discriminator == (int)ETagDiscriminator.Short);
        }

        public override void Read(short[] buffer, int startIndex)
        {
            if (CompareByteArrayToShortArray(_readbuffer, 0, buffer, startIndex, _readbuffer.Length))
            {
                return;
            }

            this.Copy(buffer, startIndex);
            this.Value = (object)buffer[startIndex];
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

        public override void Read(byte[] buffer, int startIndex)
        {
            throw new NotImplementedException();
        }

       
    }
}
