using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Contracts
{
    public abstract class Block : BaseContract, IDisposable
    {

        #region Public Properties

        public Guid DeviceID { get; set; }
        public virtual string StartAddress { get; set; }
        public virtual int BufferSize { get; set; }

        public virtual IReadOnlyList<Tag> Tags
        {
            get
            {
                return _tags;
            }
        }

        #endregion

        #region Fields

        protected ObservableCollection<Tag> _tags = new ObservableCollection<Tag>();

        #endregion

        #region Abstracts

        public abstract void Read();
        public abstract void Write();
        public abstract void ForceRead(byte[] buffer);

        #endregion

        #region Constructor

        protected Block()
        {
            _tags.CollectionChanged += tags_CollectionChanged;
        }

        protected abstract void tags_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e);

        #endregion

        #region Public Methods

        public abstract bool ValidateTag(Tag tag);

        public abstract bool AddTag(Tag tag);

        public abstract bool RemoveTag(Tag tag);

        public abstract void RemoveAllTags();

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"{Name} / {StartAddress} : {BufferSize:N0} bytes";
        }

        public override bool Equals(object obj)
        {
            return obj is Block block &&
                   ID.Equals(block.ID) &&
                   Name == block.Name &&
                   Path == block.Path &&
                   Description == block.Description &&
                   Discriminator == block.Discriminator &&
                   StartAddress == block.StartAddress &&
                   BufferSize == block.BufferSize;
        }

        public override int GetHashCode()
        {
            int hashCode = 710553078;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + Discriminator.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StartAddress);
            hashCode = hashCode * -1521134295 + BufferSize.GetHashCode();
            return hashCode;
        }

        public void Dispose()
        {
            _tags.CollectionChanged -= tags_CollectionChanged;
        }

        #endregion

    }
}
