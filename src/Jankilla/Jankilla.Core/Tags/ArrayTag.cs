using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Tags
{
    public class ArrayTag<T> : Tag, IArrayTag where T : Tag, new()
    {
        private T[] _tags;
        private byte[] _arrayWriteBuffer;
        private byte[] _arrayReadBuffer;

        public override event EventHandler<TagEventArgs> Writed;
        public override event PropertyChangedEventHandler PropertyChanged;

        
        public T[] Tags => _tags;

        [JsonIgnore]
        IEnumerable<Tag> IArrayTag.Tags => _tags;

        private int _length;
        public int Length
        {
            get => _length;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Length must be greater than 0", nameof(value));
                }

                if (_length != value)
                {
                    Initialize(value);
                }
            }
        }

        [JsonIgnore]
        public override object Value
        {
            get => _tags.Select(t => t.Value).ToArray();
            protected set
            {
                throw new NotSupportedException("Cannot set Value directly in ArrayTag");
            }
        }

        [JsonIgnore]
        public override object CalibratedValue
        {
            get => _tags.Select(t => t.CalibratedValue).ToArray();
            protected set
            {
                throw new NotSupportedException("Cannot set CalibratedValue directly in ArrayTag");
            }
        }

        public override ETagDiscriminator Discriminator => _discriminator;

        private ETagDiscriminator _discriminator;

        public ArrayTag()
        {
            _discriminator = GetDiscriminatorForType(typeof(T));
        }

        private ETagDiscriminator GetDiscriminatorForType(Type type)
        {
            if (type == typeof(BooleanTag)) return ETagDiscriminator.BooleanArray;
            if (type == typeof(IntTag)) return ETagDiscriminator.IntArray;
            if (type == typeof(ShortTag)) return ETagDiscriminator.ShortArray;
            if (type == typeof(StringTag)) return ETagDiscriminator.StringArray;
            if (type == typeof(FloatTag)) return ETagDiscriminator.FloatArray;
            if (type == typeof(UShortTag)) return ETagDiscriminator.UShortArray;
            if (type == typeof(UIntTag)) return ETagDiscriminator.UIntArray;
            if (type == typeof(LongTag)) return ETagDiscriminator.LongArray;
            if (type == typeof(ULongTag)) return ETagDiscriminator.ULongArray;
            if (type == typeof(DoubleTag)) return ETagDiscriminator.DoubleArray;
            if (type == typeof(ComplexTag)) return ETagDiscriminator.ComplexArray;

            throw new NotSupportedException($"Unsupported tag type: {type.Name}");
        }

        private void Initialize(int length)
        {
            if (_tags != null)
            {
                foreach (var tag in _tags)
                {
                    tag.Writed -= OnElementWrited;
                    tag.PropertyChanged -= OnElementPropertyChanged;
                }
            }

            _length = length;
            _tags = new T[length];

            for (int i = 0; i < length; i++)
            {
                _tags[i] = new T();
                _tags[i].ID = Guid.NewGuid();
                _tags[i].Path = $"{Path}[{i}]";
                _tags[i].Name = $"{Name}[{i + 1}]";
                _tags[i].Category = Category;
                _tags[i].Address = $"{Address}[{i}]";
                _tags[i].Writed += OnElementWrited;
                _tags[i].PropertyChanged += OnElementPropertyChanged;
            }

            ByteSize = _tags[0].ByteSize * length;

            _arrayWriteBuffer = new byte[ByteSize];
            _arrayReadBuffer = new byte[ByteSize];
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            if (_tags != null)
            {
                foreach (var tag in _tags)
                {
                    tag.Writed -= OnElementWrited;
                    tag.PropertyChanged -= OnElementPropertyChanged;
                }
            }

            _length = tags.Count();

            for (int i = 0; i < Length; i++)
            {
                if (tags.ElementAt(i).GetType() != typeof(T))
                {
                    throw new ArgumentException($"Tag at index {i} is not of type {typeof(T).Name}");
                }
                _tags[i] = (T)tags.ElementAt(i);
                _tags[i].Writed += OnElementWrited;
                _tags[i].PropertyChanged += OnElementPropertyChanged;
            }

        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                {
                    throw new IndexOutOfRangeException();
                }

                return _tags[index];
            }
        }

        public override void Read(byte[] buffer, int startIndex)
        {
            if (CompareByteArrays(_arrayReadBuffer, 0, buffer, startIndex, _arrayReadBuffer.Length))
            {
                return;
            }

            int elementSize = _tags[0].ByteSize;
            for (int i = 0; i < Length; i++)
            {
                _tags[i].Read(buffer, startIndex + (i * elementSize));
            }

            Buffer.BlockCopy(buffer, startIndex, _arrayReadBuffer, 0, ByteSize);
        }

        public override void Read(short[] buffer, int startIndex)
        {
            if (CompareByteArrayToShortArray(_arrayReadBuffer, 0, buffer, startIndex, _arrayReadBuffer.Length))
            {
                return;
            }

            int elementSize = _tags[0].ByteSize / 2; // short array이므로 ByteSize의 절반
            for (int i = 0; i < Length; i++)
            {
                _tags[i].Read(buffer, startIndex + (i * elementSize));
            }

            // short 배열을 byte 배열로 변환하여 저장
            byte[] byteBuffer = new byte[ByteSize];
            Buffer.BlockCopy(buffer, startIndex * 2, byteBuffer, 0, ByteSize);
            Buffer.BlockCopy(byteBuffer, 0, _arrayReadBuffer, 0, ByteSize);
        }

        public override void Write(object val)
        {
            throw new NotSupportedException("Use WriteElement method instead for ArrayTag");
        }

        public void WriteElement(int index, object value)
        {
            if (index < 0 || index >= Length)
            {
                throw new IndexOutOfRangeException();
            }

            _tags[index].Write(value);
        }

        public void WriteAll(object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Length != Length)
            {
                throw new ArgumentException("Values array length must match ArrayTag length");
            }

            for (int i = 0; i < Length; i++)
            {
                WriteElement(i, values[i]);
            }
        }

        private void OnElementWrited(object sender, TagEventArgs e)
        {
            T sourceTag = (T)sender;
            int elementIndex = Array.IndexOf(_tags, sourceTag);
            int offset = elementIndex * sourceTag.ByteSize;

            Buffer.BlockCopy(e.Buffer, 0, _arrayWriteBuffer, offset, sourceTag.ByteSize);

            Writed?.Invoke(this, new TagEventArgs
            {
                Address = this.Address,
                Buffer = _arrayWriteBuffer
            });
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            T sourceTag = (T)sender;
            int elementIndex = Array.IndexOf(_tags, sourceTag);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs($"Tags[{elementIndex}].{e.PropertyName}"));
        }

        public void SetFactorForAll(double factor)
        {
            foreach (var tag in _tags)
            {
                tag.Factor = factor;
                tag.UseFactor = true;
            }
        }

        public void SetOffsetForAll(double offset)
        {
            foreach (var tag in _tags)
            {
                tag.Offset = offset;
                tag.UseOffset = true;
            }
        }

        public object[] GetValues()
        {
            return _tags.Select(t => t.Value).ToArray();
        }

        public double[] GetCalibratedValues()
        {
            return _tags.Select(t => Convert.ToDouble(t.CalibratedValue)).ToArray();
        }

        public void Resize(int newLength)
        {
            Length = newLength;
        }


    }
}
