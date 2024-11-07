using Jankilla.Core.Alarms;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using Newtonsoft.Json;
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

        [JsonIgnore]
        public string StringValue { get; private set; } = string.Empty;
        [JsonIgnore]
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
        [JsonIgnore]
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
