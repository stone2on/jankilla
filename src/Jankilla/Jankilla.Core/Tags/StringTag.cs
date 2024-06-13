using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Jankilla.Core.Contracts.Tags
{
    public class StringTag : Tag
    {
        public override event EventHandler<TagEventArgs> Writed;

        public override event PropertyChangedEventHandler PropertyChanged;

        public string StringValue { get; private set; } = string.Empty;

        public override object Value
        {
            get => StringValue;
            protected set
            {
                this.StringValue = value.ToString().Replace("\0", string.Empty);
                CalibratedValue = StringValue;
                this.NotifyPropertyChanged(this.PropertyChanged, nameof(Value));
            }
        }

        private string _calibratedVal;
        public override object CalibratedValue
        {
            get => _calibratedVal;
            protected set
            {
                _calibratedVal = value.ToString();
                this.NotifyPropertyChanged(this.PropertyChanged, nameof(CalibratedValue));
            }
        }

        public override ETagDiscriminator Discriminator => ETagDiscriminator.String;

        public StringTag(string name, string address, EDirection inOut, int byteSize)
          : base(name, address, byteSize, inOut)
        {
            this._writebuffer = new byte[this.ByteSize];
        }

        public StringTag(Guid id, int no, string name, int direction, int byteSize, bool readOnly,
            string address, string category, string description, string path, string unit, bool useOffset, double offset, bool useFactor, double factor, 
            int discriminator, Guid blockID)
            : this(name, address, (EDirection)direction, byteSize)
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

            Debug.Assert(discriminator == (int)ETagDiscriminator.String);
        }

        public override void Read(short[] buffer, int startIndex)
        {
            if (CompareByteArrayToShortArray(_readbuffer, 0, buffer, startIndex, _readbuffer.Length))
            {
                return;
            }

            this.Copy(buffer, startIndex);
            this.Value = Encoding.ASCII.GetString(this._readbuffer);
        }

        public override void Write(object val)
        {
            //byte[] bytes = Encoding.ASCII.GetBytes(val.ToString());
            for (int i = 0; i < _writebuffer.Length; ++i)
            {
                _writebuffer[i] = 0;
            }

            var str = val.ToString();
            Encoding.ASCII.GetBytes(str, 0, str.Length, _writebuffer, 0);

            Writed?.Invoke(this, new TagEventArgs()
            {
                Address = this.Address,
                Buffer = _writebuffer
            });

        }

        public override void Read(byte[] buffer, int startIndex)
        {
            throw new NotImplementedException();
        }

    
    }
}
