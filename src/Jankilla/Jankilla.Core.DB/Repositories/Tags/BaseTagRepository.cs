using Dapper;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.Repositories.Tags
{
    abstract class BaseTagRepository : ITagRepository<Tag>
    {
        public string ConnectionString { get; set; }
        public BaseTagRepository(string connString)
        {
            ConnectionString = connString;
        }

        public virtual int Add(Block parent, Tag tag)
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
        public virtual int Delete(Tag tag)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                result += connection.Execute("DELETE FROM Tags WHERE ID = @ID;", tag);
                return result;
            }
        }
        public virtual int Update(Tag tag)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                result += connection.Execute("UPDATE Tags SET No = @No, Name = @Name, Direction = @Direction, " +
                    "ByteSize = @ByteSize, ReadOnly = @ReadOnly, Address = @Address, Category = @Category, " +
                    "Description = @Description, Unit = @Unit, Path = @Path, Discriminator = @Discriminator WHERE ID = @ID",
                    tag
                    );

                return result;
            }
        }

        public abstract IEnumerable<Tag> GetAll();
        public abstract IEnumerable<Tag> GetAll(Block parent);

    }
}
