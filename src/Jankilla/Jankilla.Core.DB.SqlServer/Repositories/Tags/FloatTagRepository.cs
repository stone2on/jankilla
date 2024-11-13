using Dapper;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Tags;
using Jankilla.Core.Tags.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.SqlServer.Repositories.Tags
{
    class FloatTagRepository : BaseTagRepository
    {
        public FloatTagRepository(string connString) : base(connString)
        {
        }
        public override int Add(Block parent, Tag tag)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                result += connection.Execute($@"INSERT INTO Tags (ID,No,Name,Direction,ByteSize,ReadOnly,Address,Category,Description,Path,Unit,UseOffset,Offset,UseFactor,Factor,Discriminator,BlockID) 
                                                      VALUES (@ID,@No,@Name,@Direction,@ByteSize,@ReadOnly,@Address,@Category,@Description,@Path,@Unit,@UseOffset,@Offset,@UseFactor,@Factor,@Discriminator,'{parent.ID}');",
                        tag);

                return result;
            }
        }

        public override int Delete(Tag tag)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

       
                result += connection.Execute("DELETE FROM Tags WHERE ID = @ID;",
                    tag);

                return result;
            }
        }

        public override int Update(Tag tag)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                result += connection.Execute("UPDATE Tags SET No = @No, Name = @Name, Direction = @Direction, " +
                   "ByteSize = @ByteSize, ReadOnly = @ReadOnly, Address = @Address, Category = @Category, " +
                   "Description = @Description, Unit = @Unit, Path = @Path, UseOffset = @UseOffset, Offset = @Offset, UseFactor = @UseFactor, Factor = @Factor, Discriminator = @Discriminator WHERE ID = @ID",
                   tag
                   );

                return result;
            }
        }

        public override IEnumerable<Tag> GetAll()
        {
            var tags = new List<FloatTag>();

            var sql = $"SELECT * " +
                $"FROM Tags WHERE Discriminator = {(int)ETagDiscriminator.Float}";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                tags.AddRange(connection.Query<FloatTag>(sql));
            }

            return tags;
        }

        public override IEnumerable<Tag> GetAll(Block parent)
        {
            var tags = new List<FloatTag>();

            var sql = $"SELECT * FROM Tags WHERE Discriminator = {(int)ETagDiscriminator.Float} AND BlockID = '{parent.ID}'";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                tags.AddRange(connection.Query<FloatTag>(sql));
            }

            return tags;
        }


    }
}
