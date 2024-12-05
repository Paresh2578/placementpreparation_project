using Microsoft.Data.SqlClient;
using System.Data;

namespace backend.Constant
{
    public class DbHelper
    {
        private IConfiguration configuration;

        public DbHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string getDbConnectionString()
        {
            return configuration.GetConnectionString("DBCS");
        }

        public SqlCommand getSqlCommand(string storeProcedure)
        {
            SqlConnection connection = new SqlConnection(getDbConnectionString());
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storeProcedure;

            return command;
        }

    }
}