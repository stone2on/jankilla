using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Jankilla.Core.Contracts.Tags
{
    public class BooleanTag : Tag
    {
        public override event EventHandler<TagEventArgs> Writed;

        public override event PropertyChangedEventHandler PropertyChanged;

        public bool BooleanValue { get; private set; }

        public int BitIndex { get; set; }

        private int _writeIndex = 0;
        private string _modifiedAddress;

        public override object Value
        {
            get => (object)this.BooleanValue;
            protected set
            {
                if (BooleanValue == (bool)value)
                {
                    return;
                }

                this.BooleanValue = (bool)value;
                CalibratedValue = BooleanValue;
                this.NotifyPropertyChanged(this.PropertyChanged, nameof(Value));
            }
        }

        public override ETagDiscriminator Discriminator => ETagDiscriminator.Boolean;

        private bool _calibratedVal;
        public override object CalibratedValue
        {
            get => _calibratedVal;
            protected set
            {
                _calibratedVal = (bool)value;
                this.NotifyPropertyChanged(this.PropertyChanged, nameof(CalibratedValue));
            }
        }

        public BooleanTag(
          string name,
          string address,
          EDirection inOut,
          int bitIndex)
          : base(name, address, 2, inOut)
        {
            this._writebuffer = new byte[this.ByteSize];
            this.BitIndex = bitIndex;

            if (bitIndex > 15 || bitIndex < 0)
            {
                throw new ArgumentOutOfRangeException("*bitIndex");
            }

            if (bitIndex >= 8)
            {
                _writeIndex = 1;
            }
        }
     

        public BooleanTag (Guid id, int no, string name, int direction, int byteSize, bool readOnly,
            string address, string category, string description, string path, string unit, bool useOffset, double offset, bool useFactor, double factor, 
            int discriminator, Guid blockID, int bitIndex) 
            : this (name, address, (EDirection)direction, bitIndex)
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
            Debug.Assert(discriminator == (int)ETagDiscriminator.Boolean);
        }

        public override void Read(short[] buffer, int startIndex)
        {
            this.Copy(buffer, startIndex);
            this.Value = ((uint)buffer[startIndex] & (uint)(1 << BitIndex)) > 0U;
        }

        public override void Write(object val)
        {
            Buffer.BlockCopy(_readbuffer, 0, _writebuffer, 0, ByteSize);

            if ((bool)val)
                this._writebuffer[_writeIndex] |= (byte)(1 << BitIndex);
            else
                this._writebuffer[_writeIndex] &= (byte)~(1 << BitIndex);

            _readbuffer[_writeIndex] |= _writebuffer[_writeIndex];

            Writed?.Invoke(this, new TagEventArgs()
            {
                Address = _modifiedAddress,
                Buffer = this._writebuffer
            });
        }

        public override void Read(byte[] buffer, int startIndex)
        {
            throw new NotImplementedException();
        }

        public void SetModifiedAddress(string addr)
        {
            Debug.Assert(addr != null);
            _modifiedAddress = addr;
        }

        public override bool Equals(object obj)
        {
            return obj is BooleanTag tag &&
                   base.Equals(obj) &&
                   BitIndex == tag.BitIndex;
        }

        public override int GetHashCode()
        {
            int hashCode = 102880675;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + BitIndex.GetHashCode();
            return hashCode;
        }
    }
}
