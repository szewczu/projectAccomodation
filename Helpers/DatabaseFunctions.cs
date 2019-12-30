using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Noclegi.Helpers
{
    public static class DatabaseFunctions
    {
        public static SqlConnection CreateSqlConnection()
        {
            string connectionString = @"Data Source=noclegiDB.mssql.somee.com; user id = aspr1me_SQLLogin_1; pwd = uzputoxk9x";
            SqlConnection cnn = new SqlConnection(connectionString);
            return cnn;
        }

        public static object GetFirstValueOfTheFirstColumnFromSqlCommand(string query, SqlConnection cnn)
        {
            SqlCommand command = new SqlCommand(query, cnn);
            object result = null;
            return command.ExecuteScalar() == DBNull.Value ? result : command.ExecuteScalar();
        }

    }
}
