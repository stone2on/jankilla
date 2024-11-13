using ActUtlType64Lib;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMxComponent
{
    public class MitsubishiMxComponentBlock : Block
    {
        #region Public Properties

        public override string Discriminator => "MitsubishiMxComponent";

        public override string StartAddress 
        { 
            get => _startAddress; 
            set {
                Debug.Assert(!string.IsNullOrEmpty(value));
                _startAddress = value;

                string s = null;
                DeviceCode = null;

                foreach (var dType in MitsubishiMxComponentDriver.AllDevices)
                {
                    if (_startAddress.StartsWith(dType))
                    {
                        DeviceCode = dType;
                        s = _startAddress.Substring(dType.Length);
                        break;
                    }
                }

                if (DeviceCode == null)
                {
                    throw new NotSupportedException(_startAddress);
                }

                if (MitsubishiMxComponentDriver.BitDeviceTypes.Contains(DeviceCode))
                    DeviceType = EDeviceType.Bit;
                if (MitsubishiMxComponentDriver.WordDeviceTypes.Contains(DeviceCode))
                    DeviceType = EDeviceType.Word;

                if (MitsubishiMxComponentDriver.HexDeviceTypes.Contains(DeviceCode))
                    DeviceNumber = EDeviceNumber.Hex;
                if (MitsubishiMxComponentDriver.DecimalDeviceTypes.Contains(DeviceCode))
                    DeviceNumber = EDeviceNumber.Decimal;


                if (DeviceType == EDeviceType.Unknown)
                    throw new NotSupportedException(DeviceCode);
                if (this.DeviceNumber == EDeviceNumber.Unknown)
                    throw new NotSupportedException(DeviceCode);

                StartAddressNo = DeviceNumber != EDeviceNumber.Hex ? int.Parse(s) : int.Parse(s, NumberStyles.HexNumber);

                if (this.DeviceType == EDeviceType.Bit && StartAddressNo % 16 != 0)
                {
                    throw new NotSupportedException(_startAddress);
                }
            }
        }

        public override int BufferSize 
        {
            get => _bufferSize;
            set
            {
                _bufferSize = value;

                int shortBuffSize = value / 2;

                this._readbuffer = new short[shortBuffSize];
                this._writeBuffer = new short[shortBuffSize];
                _bufferSize = shortBuffSize * 2;

                _shortBufferSize = shortBuffSize;
            }
        }
        public int StationNo 
        {
            get { return _stationNo; }
            set
            {
                if (value < 0)
                {
                    value = 1;
                }

                _stationNo = value;
                _plc.ActLogicalStationNumber = _stationNo;
            }
        }
        public int StartAddressNo { get; set; }
        public string DeviceCode { get; set; }
        public EDeviceType DeviceType { get; set; }
        public EDeviceNumber DeviceNumber { get; set; }


        #endregion

        #region Fields

        private int _stationNo;

        private short[] _readbuffer;
        private short[] _writeBuffer;
        private int _bufferSize;
        private string _startAddress;
        private ActUtlType64 _plc = new ActUtlType64();

        private IList<MitsubishiMxComponentTagWrapper> _mxTags = new List<MitsubishiMxComponentTagWrapper>();
       
        private int _shortBufferSize;

        #endregion

        #region Constructor

        
        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return obj is MitsubishiMxComponentBlock block &&
                   base.Equals(obj) &&
                   StartAddress == block.StartAddress &&
                   StationNo == block.StationNo &&
                   StartAddressNo == block.StartAddressNo &&
                   DeviceCode == block.DeviceCode &&
                   DeviceType == block.DeviceType &&
                   DeviceNumber == block.DeviceNumber;
        }

        public override int GetHashCode()
        {
            int hashCode = 2092071051;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StartAddress);
            hashCode = hashCode * -1521134295 + StationNo.GetHashCode();
            hashCode = hashCode * -1521134295 + StartAddressNo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DeviceCode);
            hashCode = hashCode * -1521134295 + DeviceType.GetHashCode();
            hashCode = hashCode * -1521134295 + DeviceNumber.GetHashCode();
            return hashCode;
        }

        public override bool ValidateTag(Tag tag)
        {
            string strNum = tag.Address.Substring(DeviceCode.Length);

            bool bParsed;
            int num;

            if (DeviceNumber != EDeviceNumber.Hex)
            {
                bParsed = int.TryParse(strNum, out num);
            }
            else
            {
                bParsed = int.TryParse(strNum, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num);
            }

            if (!bParsed)
            {
                return false;
            }

            if (!tag.Address.StartsWith(this.DeviceCode))
            {
                return false;
            }

            if (DeviceType == EDeviceType.Word)
            {
                if (num < StartAddressNo || num + (tag.ByteSize / 2) > StartAddressNo + _shortBufferSize)
                {
                    return false;
                }
            }
            else if (num < StartAddressNo || num > StartAddressNo + (_shortBufferSize * 16))
            {
                return false;
            }

            return true;
        }
     
        public override bool Open()
        {
            IsOpened = _plc.Open() == 0;

            return IsOpened;
        }

        public override void Close()
        {
            _plc.Close();
            IsOpened = false;
        }

        #endregion

        #region Events

        protected override void tags_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.tags_CollectionChanged(sender, e);

            var tags = sender as ObservableCollection<Tag>;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    var newTag = tags[e.NewStartingIndex];
                    string strNum = newTag.Address.Substring(DeviceCode.Length);
                    int num;

                    if (DeviceNumber != EDeviceNumber.Hex)
                    {
                        int.TryParse(strNum, out num);
                    }
                    else
                    {
                        int.TryParse(strNum, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num);
                    }

                    int bufferStartIndex = num - this.StartAddressNo;

                    if (DeviceType == EDeviceType.Bit)
                        bufferStartIndex /= 16;

                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                    {
                        _mxTags.Add(new MitsubishiMxComponentTagWrapper(newTag, bufferStartIndex, DeviceCode, DeviceNumber));
                        return;
                    }
                   
                    _mxTags[e.NewStartingIndex] = new MitsubishiMxComponentTagWrapper(newTag, bufferStartIndex, DeviceCode, DeviceNumber);
                    return;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    _mxTags.RemoveAt(e.OldStartingIndex);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    _mxTags.Clear();
                    break;
                default:
                    break;
            }
        }

        public override void Read()
        {
            int ret = _plc.ReadDeviceBlock2(StartAddress, _readbuffer.Length, out _readbuffer[0]);
            if (ret != 0)
            {
                return;
            }

            foreach (MitsubishiMxComponentTagWrapper mxTag in _mxTags)
            {
                mxTag.Tag.Read(this._readbuffer, mxTag.BufferStartIndex);
            }
          
        }

        public override void Write()
        {
            if (IsOpened == false)
            {
                return;
            }

            while (!_writeEventQueue.IsEmpty)
            {
                if (!_writeEventQueue.TryDequeue(out TagEventArgs result))
                {
                    continue;
                }

                int lSize = result.Buffer.Length / 2;
                if (result.Buffer.Length % 2 == 1)
                {
                    ++lSize;
                }

                Buffer.BlockCopy(result.Buffer, 0, _writeBuffer, 0, result.Buffer.Length);

                int ret = _plc.WriteDeviceBlock2(result.Address, lSize, ref _writeBuffer[0]);

                if (ret == 0)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    Debug.WriteLine($"{result.Address} : SIZE {lSize} FAILED");
                }
            }
        }

        public override void ForceRead(byte[] buffer)
        {
            Buffer.BlockCopy(buffer, 0, _readbuffer, 0, buffer.Length);

            foreach (MitsubishiMxComponentTagWrapper mxTag in _mxTags)
            {
                mxTag.Tag.Read(this._readbuffer, mxTag.BufferStartIndex);
            }
        }


        #endregion
    }
}
