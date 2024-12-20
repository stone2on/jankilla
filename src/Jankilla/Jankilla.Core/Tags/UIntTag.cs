﻿using Jankilla.Core.Alarms;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Jankilla.Core.Contracts.Tags
{
    public class UIntTag : Tag
    {
        public override event EventHandler<TagEventArgs> Writed;
        public override event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public uint UIntValue { get; private set; }
        [JsonIgnore]
        public override object Value
        {
            get => (object)this.UIntValue;
            protected set
            {
                this.UIntValue = (uint)value;
                double calibratedVal = UIntValue;
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

        public override ETagDiscriminator Discriminator => ETagDiscriminator.UInt;

        public UIntTag()
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
            this.Value = BitConverter.ToInt32(this._readbuffer, 0);
        }

        public override void Write(object val)
        {
            int num = (int)val;
            this._writebuffer[3] = (byte)(num >> 24);
            this._writebuffer[2] = (byte)(num >> 16);
            this._writebuffer[1] = (byte)(num >> 8);
            this._writebuffer[0] = (byte)num;
            EventHandler<TagEventArgs> writed = this.Writed;
            if (writed == null)
                return;
            writed(this, new TagEventArgs()
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
