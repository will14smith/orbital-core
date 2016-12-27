using System;
using System.Data.Common;
using Orbital.Data.Connections;
using Npgsql;

namespace Orbital.Web
{
    internal class PostgresqlConnectionFactory : IDbConnectionFactory
    {
        private string _connectionString;

        public PostgresqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection GetConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}