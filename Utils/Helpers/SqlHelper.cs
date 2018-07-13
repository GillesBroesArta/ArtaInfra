using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArtaInfra.Utils.Extensions;
using MySql.Data.MySqlClient;

namespace ArtaInfra.Utils.Helpers
{
    public static class SqlHelper
    {
        /// <summary>
        /// Work around for the inner join bug
        /// </summary>
        /// <param name="connectionString">Retrieves the count for the query in a separate connection</param>
        /// <param name="query">Parametrized query</param>
        /// <returns></returns>
        public static async Task<int> GetCountFromInnerJoin<T>(string connectionString, IQueryable<T> query, bool mySql = false) where T : class
        {
            //------Workaround CountAsync query
            //Workaround for Entity framework count bug on inner joins
            var fieldsRegex = @".*SELECT(.*)FROM.*";
            var qry = query.ToSql().Replace(Environment.NewLine, " ");
            var fields = Regex.Match(qry, fieldsRegex).Groups[1].Value;
            var countQuery = qry.Replace(fields, " COUNT(*) ");

            //Order by can't be used in a count qeuery
            var orderRegex = @"ORDER.*";
            countQuery = Regex.Replace(countQuery, orderRegex, " ");

            if (mySql)
                return await SqlSecondConnectionGetResultFromMySql(connectionString, countQuery);

            return await SqlSecondConnectionGetResult(connectionString, countQuery);
            //------Workaround
        }

        public static async Task<int> SqlSecondConnectionGetResult(string connectionString, string command)
        {
            int result;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var cmd = new SqlCommand(command, connection);
                result = (int)await cmd.ExecuteScalarAsync();
            }
            return result;
        }

        public static async Task<int> SqlSecondConnectionGetResultFromMySql(string connectionString, string command)
        {
            long result;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var cmd = new MySqlCommand(command, connection);
                result = (long)await cmd.ExecuteScalarAsync();
            }
            return (int)result;
        }
    }
}
