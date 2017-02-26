using System.Data;
using Dapper.Contrib.Extensions;

namespace Orbital.Data.Tests.Helpers
{
    internal class ExternallyManagedConnection : IDbConnection
    {
        private readonly IDbConnection _connection;

        static ExternallyManagedConnection()
        {
            var previous = SqlMapperExtensions.GetDatabaseType;
            SqlMapperExtensions.GetDatabaseType = connection =>
            {
                if (connection is ExternallyManagedConnection)
                {
                    connection = ((ExternallyManagedConnection)connection)._connection;
                }

                return previous?.Invoke(connection).ToLower() ?? connection.GetType().Name.ToLower();
            };
        }

        public ExternallyManagedConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        public string ConnectionString { get => _connection.ConnectionString; set => _connection.ConnectionString = value; }

        public int ConnectionTimeout => _connection.ConnectionTimeout;

        public string Database => _connection.Database;

        public ConnectionState State => _connection.State;

        public IDbTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _connection.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            _connection.Close();
        }

        public IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }

        public void Dispose()
        {
            // no-op, this is externally managed
        }

        public void Open()
        {
            _connection.Open();
        }
    }
}