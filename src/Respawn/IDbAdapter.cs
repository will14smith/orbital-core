using System.Collections.Generic;

namespace Respawn
{
    public interface IDbAdapter
    {
        string BuildTableCommandText(Checkpoint checkpoint);
        string BuildRelationshipCommandText(Checkpoint checkpoint);
        string BuildDeleteCommandText(IEnumerable<string> tablesToDelete);
    }
}