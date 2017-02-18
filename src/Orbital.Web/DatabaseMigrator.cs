using DbUp;
using Npgsql;
using Orbital.Data.Migrations;

namespace Orbital.Web
{
    class DatabaseMigrator
    {
        public static void Migrate(string connectionString)
        {
            EnsureDatabaseExists(connectionString);
            ApplyMigrations(connectionString);
        }

        private static void EnsureDatabaseExists(string connectionString)
        {
            // NOTE: this code contains SQL injections, DO NOT allow untrusted user input here

            const string checkDbExists = @"SELECT 1 FROM pg_database WHERE datname = '{0}'";
            const string createDb = @"CREATE DATABASE {0}";

            var parsedConnectionString = new NpgsqlConnectionStringBuilder(connectionString);
            var dbName = parsedConnectionString.Database;

            parsedConnectionString.Database = null;
            using (var connection = new NpgsqlConnection(parsedConnectionString.ToString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format(checkDbExists, dbName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return;
                        }

                        // create db
                        using (var command2 = connection.CreateCommand())
                        {
                            command2.CommandText = string.Format(createDb, dbName);
                            command2.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static void ApplyMigrations(string connectionString)
        {
            var builder = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .LogToConsole();

            Migrator.Migrate(builder);
        }
    }
}
