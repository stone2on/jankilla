using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Tags
{
    public class ComplexTag : Tag
    {
        private Dictionary<Guid, Tag> _tags;
        private byte[] _complexWriteBuffer;
        private byte[] _complexReadBuffer;

        public override event EventHandler<TagEventArgs> Writed;
        public override event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public IReadOnlyDictionary<Guid, Tag> Tags => _tags;

        [JsonIgnore]
        public override object Value
        {
            get => _tags;
            protected set
            {
                // ComplexTag에서는 Value를 직접 설정하지 않음
                throw new NotSupportedException("Cannot set Value directly in ComplexTag");
            }
        }

        [JsonIgnore]
        public override object CalibratedValue
        {
            get => _tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.CalibratedValue);
            protected set
            {
                // ComplexTag에서는 CalibratedValue를 직접 설정하지 않음
                throw new NotSupportedException("Cannot set CalibratedValue directly in ComplexTag");
            }
        }

        public override ETagDiscriminator Discriminator => ETagDiscriminator.Complex;

        public ComplexTag()
        {
            _tags = new Dictionary<Guid, Tag>();
            ByteSize = 0; // 초기 크기는 0, AddTag 메서드에서 동적으로 계산
        }

        public void AddTag(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            if (string.IsNullOrEmpty(tag.Name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(tag.Name));
            }

            if (_tags.ContainsKey(tag.ID))
            {
                throw new ArgumentException($"Tag with name '{tag.Name}' already exists");
            }

            _tags[tag.ID] = tag;

            ByteSize += tag.ByteSize;

            _complexWriteBuffer = new byte[ByteSize];
            _complexReadBuffer = new byte[ByteSize];

            tag.Writed += OnSubTagWrited;

            tag.PropertyChanged += OnSubTagPropertyChanged;
        }

        public void RemoveTag(Guid id)
        {
            if (_tags.TryGetValue(id, out Tag tag))
            {
                tag.Writed -= OnSubTagWrited;
                tag.PropertyChanged -= OnSubTagPropertyChanged;

                ByteSize -= tag.ByteSize;

                _tags.Remove(id);

                _complexWriteBuffer = new byte[ByteSize];
                _complexReadBuffer = new byte[ByteSize];
            }
        }

        public override void Read(byte[] buffer, int startIndex)
        {
            if (CompareByteArrays(_complexReadBuffer, 0, buffer, startIndex, _complexReadBuffer.Length))
            {
                return;
            }

            int currentIndex = startIndex;
            foreach (var tag in _tags.Values)
            {
                tag.Read(buffer, currentIndex);
                currentIndex += tag.ByteSize;
            }

            Buffer.BlockCopy(buffer, startIndex, _complexReadBuffer, 0, ByteSize);
        }

        public override void Read(short[] buffer, int startIndex)
        {
            if (CompareByteArrayToShortArray(_complexReadBuffer, 0, buffer, startIndex, _complexReadBuffer.Length))
            {
                return;
            }

            int currentIndex = startIndex;
            foreach (var tag in _tags.Values)
            {
                tag.Read(buffer, currentIndex);
                currentIndex += tag.ByteSize / 2; // short 배열이므로 ByteSize의 절반만큼 이동
            }

            // short 배열을 byte 배열로 변환하여 저장
            byte[] byteBuffer = new byte[ByteSize];
            Buffer.BlockCopy(buffer, startIndex * 2, byteBuffer, 0, ByteSize);
            Buffer.BlockCopy(byteBuffer, 0, _complexReadBuffer, 0, ByteSize);
        }

        public override void Write(object val)
        {
            throw new NotSupportedException("Use WriteTag method instead for ComplexTag");
        }

        public void WriteTag(Guid id, object value)
        {
            if (!_tags.TryGetValue(id, out Tag tag))
                throw new KeyNotFoundException($"Tag '{id}' not found");

            tag.Write(value);
        }

        private void OnSubTagWrited(object sender, TagEventArgs e)
        {
            Tag sourceTag = (Tag)sender;
            int offset = CalculateTagOffset(sourceTag);

            // 전체 버퍼에 부분 버퍼 복사
            Buffer.BlockCopy(e.Buffer, 0, _complexWriteBuffer, offset, sourceTag.ByteSize);

            // 상위 Writed 이벤트 발생
            Writed?.Invoke(this, new TagEventArgs
            {
                Address = this.Address,
                Buffer = _complexWriteBuffer
            });
        }

        private void OnSubTagPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // 하위 태그의 속성 변경을 상위로 전파
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs($"Tags[{((Tag)sender).Name}].{e.PropertyName}"));
        }

        private int CalculateTagOffset(Tag tag)
        {
            int offset = 0;
            foreach (var kvp in _tags)
            {
                if (kvp.Value == tag)
                    break;
                offset += kvp.Value.ByteSize;
            }
            return offset;
        }

      
    }
}
