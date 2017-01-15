using Orbital.Data.Connections;
using System;
using System.Data;
using Respawn;
using Xunit;

namespace Orbital.Data.Tests.Helpers
{
    public abstract class DatabaseRepositoryTestBase : IDisposable, IClassFixture<DatabaseFixture>
    {
        private readonly IDbConnection _connection;
        private readonly Checkpoint _checkpoint;

        protected DatabaseRepositoryTestBase(DatabaseFixture databaseFixture)
        {
            _connection = new ExternallyManagedConnection(databaseFixture.GetConnection());
            _checkpoint = new Checkpoint
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToExclude = new[] { "pg_catalog", "information_schema" }
            };
            _checkpoint.Reset(_connection);
        }

        protected IDbConnectionFactory GetConnectionFactory()
        {
            return new TestConnectionFactory(() => _connection);
        }

        public void Dispose()
        {
            _checkpoint.Reset(_connection);
            _connection.Dispose();
        }
    }

    internal class TestConnectionFactory : IDbConnectionFactory
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public TestConnectionFactory(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IDbConnection GetConnection()
        {
            return _connectionFactory();
        }
    }
}
