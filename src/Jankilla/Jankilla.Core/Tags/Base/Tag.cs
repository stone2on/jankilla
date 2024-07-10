using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Jankilla.Core.Contracts.Tags
{
    public abstract class Tag : IBufferRead, IBufferWrite, INotifyPropertyChanged
    {

        #region Event Handlers

        public abstract event EventHandler<TagEventArgs> Writed;
        public abstract event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public Guid ID { get; set; }

        public int No { get; set; }

        public string Name { get; set; }

        public EDirection Direction { get; set; }

        public int ByteSize 
        {
            get {  return _byteSize; } 
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(ByteSize));

                _byteSize = value;

                _readbuffer = new byte[value];
                _writebuffer = new byte[value];
            } 
        }

        public bool ReadOnly { get; set; }

        public string Address { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public Guid BlockID { get; set; }

        public abstract object Value { get; protected set; }
        public abstract object CalibratedValue { get; protected set; }
        public string Unit { get; set; }

        public bool UseFactor { get; set; }
        public double Factor { get; set; }
        public bool UseOffset { get; set; }
        public double Offset { get; set; }

        public abstract ETagDiscriminator Discriminator { get; }

        #endregion

        #region Fields


        protected byte[] _readbuffer;
        protected byte[] _writebuffer;

        private int _byteSize;

        #endregion

        #region Constructor

        protected Tag() { }

        #endregion

        #region Statics

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe int memcmp(void* b1, void* b2, long count);

        public static unsafe bool CompareByteArrays(byte[] b1, byte[] b2)
        {
            fixed (byte* buffer1 = b1, buffer2 = b2)
            {
                return b1.Length == b2.Length && memcmp(buffer1, buffer2, b1.Length) == 0;
            }
        }

        public static unsafe bool CompareByteArrays(byte[] b1, int offset1, byte[] b2, int offset2, int count)
        {
            fixed (byte* buffer1 = b1, buffer2 = b2)
            {
                return memcmp(buffer1 + offset1, buffer2 + offset2, count) == 0;
            }
        }

        public static unsafe bool CompareByteArrayToShortArray(byte[] b1, short[] b2)
        {
            fixed (byte* buffer1 = b1)
            fixed (short* buffer2 = b2)
            {
                return b1.Length == b2.Length * 2 && memcmp(buffer1, buffer2, b1.Length) == 0;
            }
        }

        public static unsafe bool CompareByteArrayToShortArray(byte[] b1, int offset1, short[] b2, int offset2, int count)
        {
            fixed (byte* buffer1 = b1)
            fixed (short* buffer2 = b2)
            {
                return memcmp(buffer1 + offset1, buffer2 + offset2, count) == 0;
            }
        }

        #endregion

        #region Public Methods

        public abstract void Read(short[] buffer, int bufferStart);
        public abstract void Read(byte[] buffer, int startIndex);
        public abstract void Write(object val);

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            return obj is Tag tag &&
                   ID.Equals(tag.ID) &&
                   No == tag.No &&
                   Name == tag.Name &&
                   Direction == tag.Direction &&
                   ByteSize == tag.ByteSize &&
                   ReadOnly == tag.ReadOnly &&
                   Address == tag.Address &&
                   Category == tag.Category &&
                   Description == tag.Description &&
                   Path == tag.Path &&
                   Unit == tag.Unit &&
                   UseFactor == tag.UseFactor &&
                   Factor == tag.Factor &&
                   UseOffset == tag.UseOffset &&
                   Offset == tag.Offset &&
                   Discriminator == tag.Discriminator;
        }

        public override int GetHashCode()
        {
            int hashCode = -2021832098;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + No.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            hashCode = hashCode * -1521134295 + ByteSize.GetHashCode();
            hashCode = hashCode * -1521134295 + ReadOnly.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Category);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Unit);
            hashCode = hashCode * -1521134295 + UseFactor.GetHashCode();
            hashCode = hashCode * -1521134295 + Factor.GetHashCode();
            hashCode = hashCode * -1521134295 + UseOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + Offset.GetHashCode();
            hashCode = hashCode * -1521134295 + Discriminator.GetHashCode();

            return hashCode;
        }

        #endregion

        #region Protected Helpers

        protected void Copy(short[] buffer, int startIndex)
        {
            Debug.Assert(buffer != null);
            Debug.Assert(buffer.Length != 0);

            Buffer.BlockCopy(buffer, startIndex * 2, _readbuffer, 0, this.ByteSize);
        }

        #endregion

        #region Events

        protected void NotifyPropertyChanged(PropertyChangedEventHandler handler, string name)
        {
            if (handler == null)
            {
                return;
            }

            handler(this, new PropertyChangedEventArgs(name));
        }

  

        #endregion

    }
}
