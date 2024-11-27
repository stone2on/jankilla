using Jankilla.Core.Collections;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
        protected ConcurrentQueue<TagEventArgs> _writeEventQueue = new ConcurrentQueue<TagEventArgs>();
        protected UniqueObservableCollection<Tag> _tags = new UniqueObservableCollection<Tag>();

        private bool disposedValue;

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
      
        #endregion

        #region Public Methods

        public virtual ValidationResult ValidateTag(Tag tag)
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

            return new ValidationResult(true, "Tag is valid");
        }

        public virtual ValidationResult AddTag(Tag tag)
        {
            ValidationResult validationResult = ValidateTag(tag);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            tag.Path = $"{Path}.{tag.Name}";
            tag.BlockID = ID;

            _tags.Add(tag);

            tag.Writed += Tag_Writed;

            return new ValidationResult(true, "Tag added successfully");
        }

        public virtual bool RemoveTag(Tag tag)
        {
            tag.Writed -= Tag_Writed;

            return _tags.Remove(tag);
        }

        public virtual void RemoveAllTags()
        {
            foreach (var tag in _tags)
            {
                tag.Writed -= Tag_Writed;
            }

            _tags.Clear();
        }

        public void ReplaceTag(int index, Tag tag)
        {
            _tags[index] = tag;
        }

        public void SuppressTagEvents(bool suppress)
        {
            foreach (Tag tag in Tags)
            {
                tag.SuppressEvents(suppress);
            }
        }
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tags.CollectionChanged -= tags_CollectionChanged;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }



        #endregion

        #region Events

        protected virtual void tags_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (Tag tag in e.NewItems)
                        {
                            tag.Path = $"{Path}.{tag.Name}";
                            tag.BlockID = ID;
                            tag.Writed += Tag_Writed;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null)
                    {
                        foreach (Tag tag in e.OldItems)
                        {
                            tag.Writed -= Tag_Writed;
                        }
                    }
                    if (e.NewItems != null)
                    {
                        foreach (Tag tag in e.NewItems)
                        {
                            tag.Path = $"{Path}.{tag.Name}";
                            tag.BlockID = ID;
                            tag.Writed += Tag_Writed;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (Tag tag in e.OldItems)
                        {
                            tag.Writed -= Tag_Writed;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (Tag tag in _tags)
                    {
                        tag.Writed -= Tag_Writed;
                    }
                    break;
                default:
                    break;

            }
        }

        private void Tag_Writed(object sender, TagEventArgs e)
        {
            _writeEventQueue.Enqueue(e);
        }

        #endregion

    }
}
