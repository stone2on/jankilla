using Dapper;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Tags.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.Repositories.Tags
{
    class BooleanTagRepository : BaseTagRepository
    {
        public BooleanTagRepository(string connString) : base(connString)
        {
        }

        public override int Add(Block parent, Tag tag)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;


                    result += connection.Execute($@"INSERT INTO Tags (ID,No,Name,Direction,ByteSize,ReadOnly,Address,Category,Description,Path,Unit,Discriminator,BlockID) 
                                                      VALUES (@ID,@No,@Name,@Direction,@ByteSize,@ReadOnly,@Address,@Category,@Description,@Path,@Unit,@Discriminator,'{parent.ID}');",
                        tag);
               
                    result += connection.Execute($@"INSERT INTO BooleanTags (ID,BitIndex) 
                                                      VALUES (@ID,@BitIndex);",
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

                //result += connection.Execute("DELETE FROM BooleanTagValues WHERE TagID = @ID;",
                //    tag);

                result += connection.Execute("DELETE FROM BooleanTags WHERE ID = @ID;",
                    tag);

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


                result += connection.Execute("UPDATE BooleanTags SET BitIndex = @BitIndex WHERE ID = @ID",
                    tag);

                result += connection.Execute("UPDATE Tags SET No = @No, Name = @Name, Direction = @Direction, " +
                   "ByteSize = @ByteSize, ReadOnly = @ReadOnly, Address = @Address, Category = @Category, " +
                   "Description = @Description, Unit = @Unit, Path = @Path, Discriminator = @Discriminator WHERE ID = @ID",
                   tag
                   );



                return result;
            }
        }

        public override IEnumerable<Tag> GetAll()
        {
            var tags = new List<BooleanTag>();

            var sql = $"SELECT t.*, b.BitIndex FROM Tags t LEFT JOIN BooleanTags b ON t.ID = b.ID WHERE t.Discriminator = {(int)ETagDiscriminator.Boolean}";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                tags.AddRange(connection.Query<BooleanTag>(sql));
            }

            return tags;
        }

        public override IEnumerable<Tag> GetAll(Block parent)
        {
            var tags = new List<BooleanTag>();

            var sql = $"SELECT t.*, b.BitIndex FROM Tags t LEFT JOIN BooleanTags b ON t.ID = b.ID WHERE t.Discriminator = {(int)ETagDiscriminator.Boolean} AND t.BlockID = '{parent.ID}'";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                tags.AddRange(connection.Query<BooleanTag>(sql));
            }

            return tags;
        }

    
    }
}
