using Dapper;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.DB.Repositories.Tags;
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
    class StringTagRepository : BaseTagRepository
    {
        public StringTagRepository(string connString) : base(connString)
        {
        }


        public override IEnumerable<Tag> GetAll()
        {
            var tags = new List<StringTag>();

            var sql = $"SELECT * FROM Tags WHERE Discriminator = {(int)ETagDiscriminator.String}";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                tags.AddRange(connection.Query<StringTag>(sql));
            }

            return tags;
        }

        public override IEnumerable<Tag> GetAll(Block parent)
        {
            var tags = new List<StringTag>();

            var sql = $"SELECT * FROM Tags WHERE Discriminator = {(int)ETagDiscriminator.String} AND BlockID = '{parent.ID}'";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                tags.AddRange(connection.Query<StringTag>(sql));
            }

            return tags;
        }

        public override int Delete(Tag tag)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                //result += connection.Execute("DELETE FROM StringTagValues WHERE TagID = @ID;",
                //tag);

                result += connection.Execute("DELETE FROM Tags WHERE ID = @ID;", tag);
                return result;
            }
        }
    }
}
