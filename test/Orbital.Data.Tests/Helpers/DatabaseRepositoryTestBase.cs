using Orbital.Data.Connections;
using System;
using System.Data;
using Xunit;

namespace Orbital.Data.Tests.Helpers
{
    public abstract class DatabaseRepositoryTestBase : IDisposable, IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _databaseFixture;
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        protected DatabaseRepositoryTestBase(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
            _connection = new ExternallyManagedConnection(databaseFixture.GetConnection());
            _transaction = _connection.BeginTransaction(IsolationLevel.Snapshot);
        }

        protected IDbConnectionFactory GetConnectionFactory()
        {
            return new TestConnectionFactory(() => _connection);
        }

        public void Dispose()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _connection.Dispose();
        }
    }

    internal class TestConnectionFactory : IDbConnectionFactory
    {
        private Func<IDbConnection> _connectionFactory;

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
