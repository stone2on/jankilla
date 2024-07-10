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

        private int _bitIndex;
        public int BitIndex 
        { 
            get {  return _bitIndex; }
            set 
            {
                if (value > 15 || value < 0)
                {
                    throw new ArgumentOutOfRangeException("*bitIndex");
                }

                if (value >= 8)
                {
                    _writeIndex = 1;
                }

                _bitIndex = value; 
            }
        }

        public bool BooleanValue { get; private set; }

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

        private int _writeIndex = 0;
        private string _modifiedAddress;

        public BooleanTag()
        {
            ByteSize = 2;
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
