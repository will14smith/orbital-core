using Orbital.Data.Connections;
using Npgsql;
using System.Data;

namespace Orbital.Web
{
    internal class PostgresqlConnectionFactory : IDbConnectionFactory
    {
        private string _connectionString;

        public PostgresqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}