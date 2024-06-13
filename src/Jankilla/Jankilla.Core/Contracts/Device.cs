using System;
using System.Collections.Generic;

namespace Jankilla.Core.Contracts
{
    public abstract class Device : BaseContract
    {
        #region Public Properties

        public Guid DriverID { get; set; }

        #endregion

        #region Fields


        #endregion

        #region Constructor

        #endregion

        #region Public Methods

        public abstract IReadOnlyList<Block> Blocks { get; }

        public abstract bool ValidateBlock(Block block);

        public abstract bool AddBlock(Block block);
        
        public abstract bool RemoveBlock(Block block);

        public abstract void RemoveAllBlocks();

        public abstract void ReplaceBlock(int index, Block block);

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

        #endregion

        #region Overrides



        #endregion
    }
}
