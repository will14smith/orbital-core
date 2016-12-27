using System.Data.Common;

namespace Orbital.Data.Connections
{
    public interface IDbConnectionFactory
    {
        DbConnection GetConnection();
    }
}
