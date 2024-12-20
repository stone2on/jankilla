﻿using Jankilla.Core.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jankilla.Core.Contracts
{
    public abstract class Device : BaseContract, IDisposable
    {
        #region Public Properties
        public IReadOnlyList<Block> Blocks
        {
            get
            {
                return _blocks;
            }
        }

        public Guid DriverID { get; set; }

        #endregion

        #region Fields

        protected UniqueObservableCollection<Block> _blocks = new UniqueObservableCollection<Block>();
        private bool disposedValue;

        #endregion

        #region Constructor

        protected Device()
        {
            _blocks.CollectionChanged += blocks_CollectionChanged;
        }

        protected virtual void blocks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (Block block in e.NewItems)
                        {
                            block.Path = $"{Path}.{block.Name}";
                            block.DeviceID = ID;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null)
                    {
       
                    }
                    if (e.NewItems != null)
                    {
                        foreach (Block block in e.NewItems)
                        {
                            block.Path = $"{Path}.{block.Name}";
                            block.DeviceID = ID;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (Block block in e.OldItems)
                        {
                            block.RemoveAllTags();
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    RemoveAllBlocks();
                    break;
                default:
                    break;

            }
        }

        #endregion

        #region Public Methods

        public virtual bool ValidateBlock(Block block)
        {
            bool bValidated = ValidateContract(block);

            if (bValidated == false)
            {
                return false;
            }

            if (block.Discriminator != Discriminator)
            {
                return false;
            }

            if (_blocks.Contains(block))
            {
                return false;
            }

            if (string.IsNullOrEmpty(block.StartAddress))
            {
                return false;
            }

            return true;
        }

        public virtual bool AddBlock(Block block)
        {
            bool bValidated = ValidateBlock(block);

            if (bValidated == false)
            {
                return false;
            }

            block.Path = $"{Path}.{block.Name}";
            block.DeviceID = ID;

            _blocks.Add(block);

            return true;
        }

        public virtual bool RemoveBlock(Block block)
        {
            block.RemoveAllTags();

            return _blocks.Remove(block);
        }

        public virtual void RemoveAllBlocks()
        {
            foreach (var block in _blocks)
            {
                block.RemoveAllTags();
            }
            _blocks.Clear();
        }

        public virtual bool ReplaceBlock(int index, Block block)
        {
            bool bValidated = ValidateBlock(block);
            if (bValidated == false)
            {
                return false;
            }

            _blocks[index] = block;
            return true;
        }

        #endregion

        #region Overrides

        public override bool Open()
        {
            foreach (var block in _blocks)
            {
                block.Open();
            }

            IsOpened = !_blocks.Any(b => b.IsOpened == false);

            return IsOpened;
        }

        public override void Close()
        {
            foreach (var block in _blocks)
            {
                block.Close();
            }

            IsOpened = false;
        }

        public override bool Equals(object obj)
        {
            return obj is Device device &&
                   ID.Equals(device.ID) &&
                   Name == device.Name &&
                   Path == device.Path &&
                   Description == device.Description &&
                   Discriminator == device.Discriminator;
        }

        public override int GetHashCode()
        {
            int hashCode = 1432158563;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + Discriminator.GetHashCode();
            return hashCode;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _blocks.CollectionChanged -= blocks_CollectionChanged;
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
    }
}
