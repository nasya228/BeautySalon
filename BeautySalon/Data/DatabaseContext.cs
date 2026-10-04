using System.Configuration;
using System.Data.SqlClient;

namespace BeautySalon.Data
{
    public static class DatabaseContext
    {
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["BeautySalonDB"].ConnectionString;

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}