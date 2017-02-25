using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Models.Repositories
{
    public interface IRecordRepository : IRepository<Record>
    {
        IReadOnlyCollection<Record> GetAllByClubId(int clubId);
        IReadOnlyCollection<Record> GetAllByRoundId(int roundId);
    }
}
