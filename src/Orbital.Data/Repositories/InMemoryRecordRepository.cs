using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryRecordRepository : InMemoryRepository<Record>, IRecordRepository
    {
        private InMemoryRecordRepository(List<Record> data) : base(data) { }

        public static InMemoryRecordRepository New(params Record[] records)
        {
            return new InMemoryRecordRepository(records.ToList());
        }

        public IReadOnlyCollection<Record> GetAllByClubId(int clubId) => Data.Where(x => x.Clubs?.Any(c => c.ClubId == clubId) ?? false).ToList();
        public IReadOnlyCollection<Record> GetAllByRoundId(int roundId) => Data.Where(x => x.Rounds?.Any(r => r.RoundId == roundId) ?? false).ToList();

        protected override int GetId(Record item) => item.Id;
    }
}