using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Driver.MitsubishiMcProtocol.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMcProtocol
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

#pragma warning disable CS0169 // 'MitsubishiMcProtocolBlock._stationNo' 필드가 사용되지 않았습니다.
        private int _stationNo;
#pragma warning restore CS0169 // 'MitsubishiMcProtocolBlock._stationNo' 필드가 사용되지 않았습니다.

#pragma warning disable CS0649 // 'MitsubishiMcProtocolBlock._readbuffer' 필드에는 할당되지 않으므로 항상 null 기본값을 사용합니다.
        private readonly short[] _readbuffer;
#pragma warning restore CS0649 // 'MitsubishiMcProtocolBlock._readbuffer' 필드에는 할당되지 않으므로 항상 null 기본값을 사용합니다.
#pragma warning disable CS0649 // 'MitsubishiMcProtocolBlock._writeBuffer' 필드에는 할당되지 않으므로 항상 null 기본값을 사용합니다.
        private readonly short[] _writeBuffer;
#pragma warning restore CS0649 // 'MitsubishiMcProtocolBlock._writeBuffer' 필드에는 할당되지 않으므로 항상 null 기본값을 사용합니다.

        private IList<MitsubishiMcProtocolTagWrapper> _mcTags = new List<MitsubishiMcProtocolTagWrapper>();
        private ConcurrentQueue<TagEventArgs> _writeEventQueue = new ConcurrentQueue<TagEventArgs>();

#pragma warning disable CS0649 // 'MitsubishiMcProtocolBlock._shortBufferSize' 필드에는 할당되지 않으므로 항상 0 기본값을 사용합니다.
        private int _shortBufferSize;
#pragma warning restore CS0649 // 'MitsubishiMcProtocolBlock._shortBufferSize' 필드에는 할당되지 않으므로 항상 0 기본값을 사용합니다.

        #endregion

        public override bool AddTag(Tag tag)
        {
            bool bValidated = ValidateTag(tag);

            if (bValidated == false)
            {
                return false;
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

            tag.Path = Path;
            tag.BlockID = ID;

            _tags.Add(tag);

            tag.Writed += Tag_Writed;

            return true;
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

        public override bool ValidateTag(Tag tag)
        {
            return true; 
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
