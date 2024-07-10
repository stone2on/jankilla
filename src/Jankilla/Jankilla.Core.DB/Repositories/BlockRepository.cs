using Jankilla.Core.Contracts;
using Jankilla.Driver.MitsubishiMxComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.Repositories
{
    class BlockRepository
    {
        public string ConnectionString { get; set; }

        private MitsubishiMxComponentBlockRepository MitsubishiMxComponentBlockRepo { get; set; }

        public BlockRepository(string connString)
        {
            ConnectionString = connString;

            MitsubishiMxComponentBlockRepo = new MitsubishiMxComponentBlockRepository(connString);
        }

        public int Add(Contracts.Device parent, Block block)
        {
            switch (block.Discriminator)
            {
                case "MitsubishiMxComponent":
                    return MitsubishiMxComponentBlockRepo.Add(parent, (MitsubishiMxComponentBlock)block);
                default:
                    throw new NotSupportedException();
            }
        }

        public int Delete(Block block)
        {
            switch (block.Discriminator)
            {
                case "MitsubishiMxComponent":
                    return MitsubishiMxComponentBlockRepo.Delete((MitsubishiMxComponentBlock)block);
                default:
                    throw new NotSupportedException();
            }
        }

        public IEnumerable<Block> GetAll()
        {
            var drivers = new List<Block>();
            drivers.AddRange(MitsubishiMxComponentBlockRepo.GetAll());

            return drivers;
        }

        public IEnumerable<Block> GetAll(Device parent)
        {
            var drivers = new List<Block>();
            drivers.AddRange(MitsubishiMxComponentBlockRepo.GetAll(parent));

            return drivers;
        }

        public int Update(Block block)
        {
            switch (block.Discriminator)
            {
                case "MitsubishiMxComponent":
                    return MitsubishiMxComponentBlockRepo.Update((MitsubishiMxComponentBlock)block);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
