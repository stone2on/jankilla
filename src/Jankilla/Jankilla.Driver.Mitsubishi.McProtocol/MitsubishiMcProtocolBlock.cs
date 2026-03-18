using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Utils;
using Jankilla.Driver.Mitsubishi.McProtocol.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.Mitsubishi.McProtocol
{
    public class MitsubishiMcProtocolBlock : Block
    {
        #region Public Properties

        public override string Discriminator => "MitsubishiMcProtocol";

        public override string StartAddress
        {
            get => base.StartAddress;
            set =>
                base.StartAddress = value;
        }

        public int StartAddressNo { get; set; }
        public string DeviceCode { get; set; }
        public EDeviceType DeviceType { get; set; }
        public EDeviceNumber DeviceNumber { get; set; }

        public McProtocolBase DeviceProtocol { get; set; }

        #endregion

        #region Fields

#pragma warning disable CS0169 // 'Mitsubishi.McProtocolBlock._stationNo' 필드가 사용되지 않았습니다.
        private int _stationNo;
#pragma warning restore CS0169 // 'Mitsubishi.McProtocolBlock._stationNo' 필드가 사용되지 않았습니다.

#pragma warning disable CS0649 // 'Mitsubishi.McProtocolBlock._readbuffer' 필드에는 할당되지 않으므로 항상 null 기본값을 사용합니다.
        private readonly short[] _readbuffer;
#pragma warning restore CS0649 // 'Mitsubishi.McProtocolBlock._readbuffer' 필드에는 할당되지 않으므로 항상 null 기본값을 사용합니다.
#pragma warning disable CS0649 // 'Mitsubishi.McProtocolBlock._writeBuffer' 필드에는 할당되지 않으므로 항상 null 기본값을 사용합니다.
        private readonly short[] _writeBuffer;
#pragma warning restore CS0649 // 'Mitsubishi.McProtocolBlock._writeBuffer' 필드에는 할당되지 않으므로 항상 null 기본값을 사용합니다.

        private IList<MitsubishiMcProtocolTagWrapper> _mcTags = new List<MitsubishiMcProtocolTagWrapper>();
    
#pragma warning disable CS0649 // 'Mitsubishi.McProtocolBlock._shortBufferSize' 필드에는 할당되지 않으므로 항상 0 기본값을 사용합니다.
        private int _shortBufferSize;
#pragma warning restore CS0649 // 'Mitsubishi.McProtocolBlock._shortBufferSize' 필드에는 할당되지 않으므로 항상 0 기본값을 사용합니다.

        #endregion

        public override ValidationResult AddTag(Tag tag)
        {
            ValidationResult validationResult = ValidateTag(tag);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

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
                return new ValidationResult(false, "Address parsing failed");
            }

            if (!tag.Address.StartsWith(this.DeviceCode))
            {
                return new ValidationResult(false, "Address does not start with the correct device code");
            }

            if (DeviceType == EDeviceType.Word)
            {
                if (num < StartAddressNo || num + (tag.ByteSize / 2) > StartAddressNo + _shortBufferSize)
                {
                    return new ValidationResult(false, "Address is out of range for word device type");
                }
            }
            else if (num < StartAddressNo || num > StartAddressNo + (_shortBufferSize * 16))
            {
                return new ValidationResult(false, "Address is out of range for bit device type");
            }

            tag.Path = $"{Path}.{tag.Name}";
            tag.BlockID = ID;

            _tags.Add(tag);

            tag.Writed += Tag_Writed;

            return new ValidationResult(true, "Tag added successfully");
        }

        public override void Close()
        {
            IsOpened = false;
        }

        public override void ForceRead(byte[] buffer)
        {
            Buffer.BlockCopy(buffer, 0, _readbuffer, 0, buffer.Length);

            foreach (MitsubishiMcProtocolTagWrapper mcTag in _mcTags)
            {
                mcTag.Tag.Read(this._readbuffer, mcTag.BufferStartIndex);
            }
        }

        public override bool Open()
        {
            IsOpened = true;

            return IsOpened;
        }

        public override void Read()
        {
            //int ret = _plc.ReadDeviceBlock2(StartAddress, _readbuffer.Length, out _readbuffer[0]);
            //if (ret != 0)
            //{
            //    return;
            //}

            //foreach (var mcTag in _mcTags)
            //{
            //    mcTag.Tag.Read(this._readbuffer, mcTag.BufferStartIndex);
            //}
        }

        public override void RemoveAllTags()
        {
            foreach (var tag in _tags)
            {
                tag.Writed -= Tag_Writed;
            }

            _tags.Clear();
        }

        public override bool RemoveTag(Tag tag)
        {
            tag.Writed -= Tag_Writed;

            return _tags.Remove(tag);
        }

        public override ValidationResult ValidateTag(Tag tag)
        {
            if (tag == null)
            {
                return new ValidationResult(false, "Tag is null");
            }

            if (string.IsNullOrEmpty(tag.Name))
            {
                return new ValidationResult(false, "Tag name is null or empty");
            }

            if (_tags.Contains(tag))
            {
                return new ValidationResult(false, "Tag already exists in the collection");
            }

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
                return new ValidationResult(false, "Address parsing failed");
            }

            if (!tag.Address.StartsWith(this.DeviceCode))
            {
                return new ValidationResult(false, "Address does not start with the correct device code");
            }

            if (DeviceType == EDeviceType.Word)
            {
                if (num < StartAddressNo || num + (tag.ByteSize / 2) > StartAddressNo + _shortBufferSize)
                {
                    return new ValidationResult(false, "Address is out of range for word device type");
                }
            }
            else if (num < StartAddressNo || num > StartAddressNo + (_shortBufferSize * 16))
            {
                return new ValidationResult(false, "Address is out of range for bit device type");
            }

            return new ValidationResult(true, "Tag is valid");
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

                //int ret = _plc.WriteDeviceBlock2(result.Address, lSize, ref _writeBuffer[0]);

                //if (ret == 0)
                //{
                //    Thread.Sleep(1);
                //}
                //else
                //{
                //    Console.WriteLine($"{result.Address} : SIZE {lSize} FAILED");
                //}
            }
        }

        private void Tag_Writed(object sender, TagEventArgs e)
        {
            _writeEventQueue.Enqueue(e);
        }

        protected override void tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
