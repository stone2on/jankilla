using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.DB.SqlServer.Repositories.Tags;
using Jankilla.Core.Tags.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.SqlServer.Repositories
{
    class TagRepository
    {
        public string ConnectionString { get; set; }

        private StringTagRepository StringTagRepo { get; set; }
        private IntTagRepository IntTagRepo { get; set; }
        private ShortTagRepository ShortTagRepo { get; set; }
        private BooleanTagRepository BooleanTagRepo { get; set; }
        private FloatTagRepository FloatTagRepo { get; set; }

        public TagRepository(string connString)
        {
            ConnectionString = connString;

            StringTagRepo = new StringTagRepository(connString);
            IntTagRepo = new IntTagRepository(connString);
            ShortTagRepo = new ShortTagRepository(connString);
            BooleanTagRepo = new BooleanTagRepository(connString);
            FloatTagRepo = new FloatTagRepository(connString);
        }

        public int Add(Block parent, Tag tag)
        {
            int ret;
            switch (tag.Discriminator)
            {
                case ETagDiscriminator.Boolean:
                    ret = BooleanTagRepo.Add(parent, tag);
                    break;
                case ETagDiscriminator.Int:
                    ret = IntTagRepo.Add(parent, tag);
                    break;
                case ETagDiscriminator.Short:
                    ret = ShortTagRepo.Add(parent, tag);
                    break;
                case ETagDiscriminator.String:
                    ret = StringTagRepo.Add(parent, tag);
                    break;
                case ETagDiscriminator.Float:
                    ret = FloatTagRepo.Add(parent, tag);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return ret;
        }

        public int Delete(Tag tag)
        {
            int ret;
            switch (tag.Discriminator)
            {
                case ETagDiscriminator.Boolean:
                    ret = BooleanTagRepo.Delete(tag);
                    break;
                case ETagDiscriminator.Int:
                    ret = IntTagRepo.Delete(tag);
                    break;
                case ETagDiscriminator.Short:
                    ret = ShortTagRepo.Delete(tag);
                    break;
                case ETagDiscriminator.String:
                    ret = StringTagRepo.Delete(tag);
                    break;
                case ETagDiscriminator.Float:
                    ret = FloatTagRepo.Delete(tag);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return ret;
        }

        public int Update(Tag tag)
        {
            int ret;
            switch (tag.Discriminator)
            {
                case ETagDiscriminator.Boolean:
                    ret = BooleanTagRepo.Update(tag);
                    break;
                case ETagDiscriminator.Int:
                    ret = IntTagRepo.Update(tag);
                    break;
                case ETagDiscriminator.Short:
                    ret = ShortTagRepo.Update(tag);
                    break;
                case ETagDiscriminator.String:
                    ret = StringTagRepo.Update(tag);
                    break;
                case ETagDiscriminator.Float:
                    ret = FloatTagRepo.Update(tag);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return ret;
        }

        public IEnumerable<Tag> GetAll()
        {
            var tags = new List<Tag>();
            tags.AddRange(BooleanTagRepo.GetAll());
            tags.AddRange(IntTagRepo.GetAll());
            tags.AddRange(ShortTagRepo.GetAll());
            tags.AddRange(StringTagRepo.GetAll());
            tags.AddRange(FloatTagRepo.GetAll());

            return tags;
        }
        public IEnumerable<Tag> GetAll(Block parent)
        {
            var tags = new List<Tag>();
            tags.AddRange(BooleanTagRepo.GetAll(parent));
            tags.AddRange(IntTagRepo.GetAll(parent));
            tags.AddRange(ShortTagRepo.GetAll(parent));
            tags.AddRange(StringTagRepo.GetAll(parent));
            tags.AddRange(FloatTagRepo.GetAll(parent));

            return tags;
        }
    }
}
