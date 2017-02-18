using DbUp;
using Npgsql;
using Orbital.Data.Migrations;
using System;
using System.Data;

namespace Orbital.Data.Tests.Helpers
{
    public class DatabaseFixture : IDisposable
    {
        private static readonly string BaseConnectionString = "Host=localhost;Username=orbital;Password=orbital";
        private readonly NpgsqlConnectionStringBuilder _connectionString;

        public DatabaseFixture()
        {
            _connectionString = GenerateConnectionString();

            CreateDatabase(_connectionString);
            MigrateDatabase(_connectionString);
        }

        public IDbConnection GetConnection()
        {
            var connection = new NpgsqlConnection(_connectionString.ToString());
            connection.Open();
            return connection;
        }

        private static NpgsqlConnectionStringBuilder GenerateConnectionString()
        {
            var dbName = "orbital_test_" + Guid.NewGuid().ToString("N");
            return new NpgsqlConnectionStringBuilder(BaseConnectionString)
            {
                Database = dbName
            };
        }

        private static void CreateDatabase(NpgsqlConnectionStringBuilder connectionString)
        {
            using (var global = GetGlobalConnection())
            using (var command = global.CreateCommand())
            {
                command.CommandText = string.Format(@"CREATE DATABASE {0}", connectionString.Database);
                command.ExecuteNonQuery();
            }
        }
        private static void MigrateDatabase(NpgsqlConnectionStringBuilder connectionString)
        {
            var builder = DeployChanges.To
                .PostgresqlDatabase(connectionString.ToString());
            Migrator.Migrate(builder, includeSeedData: false);
        }

        private static void DropConnections(NpgsqlConnectionStringBuilder connectionString)
        {
            using (var global = GetGlobalConnection())
            using (var command = global.CreateCommand())
            {
                command.CommandText = string.Format(@"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '{0}' AND pid <> pg_backend_pid()", connectionString.Database);
                command.ExecuteNonQuery();
            }

        }

        private static void DropDatabase(NpgsqlConnectionStringBuilder connectionString)
        {
            using (var global = GetGlobalConnection())
            using (var command = global.CreateCommand())
            {
                command.CommandText = string.Format(@"DROP DATABASE {0}", connectionString.Database);
                command.ExecuteNonQuery();
            }
        }

        private static NpgsqlConnection GetGlobalConnection()
        {
            var parsedConnectionString = new NpgsqlConnectionStringBuilder(BaseConnectionString)
            {
                Database = null
            };

            var connection = new NpgsqlConnection(parsedConnectionString.ToString());
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            DropConnections(_connectionString);
            DropDatabase(_connectionString);
        }
    }
}
