using System.Data;

namespace Orbital.Data.Connections
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
