using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.Mitsubishi.MxComponent
{
    public class MitsubishiMxComponentSimulatorBlock : Block
    {
        #region Public Properties

        public override string Discriminator => "MitsubishiMxComponentSimulator";

        public override string StartAddress
        {
            get => _startAddress;
            set
            {
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

                this._readBuffer = new short[shortBuffSize];
                this._writeBuffer = new short[shortBuffSize];
                _bufferSize = shortBuffSize * 2;

                _shortBufferSize = shortBuffSize;

                // 시뮬레이터 메모리 초기화
                InitializeMemory();
            }
        }

        public int StationNo { get; set; }
        public int StartAddressNo { get; set; }
        public string DeviceCode { get; set; }
        public EDeviceType DeviceType { get; set; }
        public EDeviceNumber DeviceNumber { get; set; }

        #endregion

        #region Fields

        private string _startAddress;
        private int _bufferSize;
        private short[] _readBuffer;
        private short[] _writeBuffer;
        private int _shortBufferSize;
        private Dictionary<string, short[]> _deviceMemory;
        private IList<MitsubishiMxComponentTagWrapper> _mxTags = new List<MitsubishiMxComponentTagWrapper>();

        #endregion

        #region Constructor

        public MitsubishiMxComponentSimulatorBlock()
        {
            _deviceMemory = new Dictionary<string, short[]>();
        }

        #endregion

        #region Public Methods

        public override bool Open()
        {
            IsOpened = true;
            return IsOpened;
        }

        public override void Close()
        {
            IsOpened = false;
        }

        public override void Read()
        {
            if (!IsOpened)
                return;

            // 시뮬레이터 메모리에서 읽기
            if (_deviceMemory.TryGetValue(DeviceCode, out var memory))
            {
                Array.Copy(memory, StartAddressNo, _readBuffer, 0, _shortBufferSize);
            }

            // 각 태그에 대해 읽기 작업 수행
            foreach (MitsubishiMxComponentTagWrapper mxTag in _mxTags)
            {
                mxTag.Tag.Read(this._readBuffer, mxTag.BufferStartIndex);
            }
        }

        public override void Write()
        {
            if (!IsOpened)
                return;

            while (!_writeEventQueue.IsEmpty)
            {
                if (!_writeEventQueue.TryDequeue(out TagEventArgs result))
                {
                    continue;
                }

                // 주소 파싱
                string deviceCode = new string(result.Address.TakeWhile(c => !char.IsDigit(c)).ToArray());
                string addressNum = result.Address.Substring(deviceCode.Length);
                int addressNo = DeviceNumber != EDeviceNumber.Hex ?
                    int.Parse(addressNum) :
                    int.Parse(addressNum, NumberStyles.HexNumber);

                // 버퍼 크기 계산
                int lSize = result.Buffer.Length / 2;
                if (result.Buffer.Length % 2 == 1)
                {
                    ++lSize;
                }

                // 쓰기 버퍼에 복사
                Buffer.BlockCopy(result.Buffer, 0, _writeBuffer, 0, result.Buffer.Length);

                // 시뮬레이터 메모리에 쓰기
                if (!_deviceMemory.ContainsKey(deviceCode))
                {
                    _deviceMemory[deviceCode] = new short[65536];
                }

                Array.Copy(_writeBuffer, 0, _deviceMemory[deviceCode], addressNo, lSize);

                Thread.Sleep(1); // 실제 통신 지연 시뮬레이션
            }
        }

        public override void ForceRead(byte[] buffer)
        {
            Buffer.BlockCopy(buffer, 0, _readBuffer, 0, buffer.Length);

            foreach (MitsubishiMxComponentTagWrapper mxTag in _mxTags)
            {
                mxTag.Tag.Read(this._readBuffer, mxTag.BufferStartIndex);
            }
        }

        public override ValidationResult ValidateTag(Tag tag)
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
                return new ValidationResult(false, "Invalid address format.");
            }

            if (!tag.Address.StartsWith(this.DeviceCode))
            {
                return new ValidationResult(false, "Address does not start with the correct device code.");
            }

            if (DeviceType == EDeviceType.Word)
            {
                if (num < StartAddressNo || num + (tag.ByteSize / 2) > StartAddressNo + _shortBufferSize)
                {
                    return new ValidationResult(false, "Address out of range for word device.");
                }
            }
            else if (num < StartAddressNo || num > StartAddressNo + (_shortBufferSize * 16))
            {
                return new ValidationResult(false, "Address out of range for bit device.");
            }

            return new ValidationResult(true, "Tag is valid.");
        }

        #endregion

        #region Protected Methods

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
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    _mxTags.Clear();
                    break;
            }
        }

        #endregion

        #region Private Methods

        private void InitializeMemory()
        {
            if (!_deviceMemory.ContainsKey(DeviceCode))
            {
                _deviceMemory[DeviceCode] = new short[65536]; // 충분히 큰 크기로 초기화
            }
        }

        // 시뮬레이터 전용 메서드들
        public void SetDeviceValue(string address, short value)
        {
            string deviceCode = new string(address.TakeWhile(c => !char.IsDigit(c)).ToArray());
            string addressNum = address.Substring(deviceCode.Length);
            int addressNo = int.Parse(addressNum);

            if (!_deviceMemory.ContainsKey(deviceCode))
            {
                _deviceMemory[deviceCode] = new short[65536];
            }

            _deviceMemory[deviceCode][addressNo] = value;
        }

        public short GetDeviceValue(string address)
        {
            string deviceCode = new string(address.TakeWhile(c => !char.IsDigit(c)).ToArray());
            string addressNum = address.Substring(deviceCode.Length);
            int addressNo = int.Parse(addressNum);

            if (!_deviceMemory.TryGetValue(deviceCode, out var memory))
            {
                return 0;
            }

            return memory[addressNo];
        }

        public void Reset()
        {
            _deviceMemory.Clear();
            InitializeMemory();
        }

        #endregion
    }
}
