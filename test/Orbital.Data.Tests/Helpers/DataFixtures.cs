using Orbital.Data.Connections;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;

namespace Orbital.Data.Tests.Helpers
{
    internal static class DataFixtures
    {
        private static int _badgeCounter = 1;
        public static int GetBadge(IDbConnectionFactory connectionFactory)
        {
            var badge = new Badge(0, "Badge" + _badgeCounter++, null, null, null, true, null);
            return new DatabaseBadgeRepository(connectionFactory).Create(badge).Id;
        }

        private static int _clubCounter = 1;
        public static int GetClub(IDbConnectionFactory connectionFactory)
        {
            var club = new Club(0, "Club" + _clubCounter++);
            return new DatabaseClubRepository(connectionFactory).Create(club).Id;
        }

        private static int _personCounter = 1;
        public static int GetPerson(IDbConnectionFactory connectionFactory)
        {
            var person = new Person(0, GetClub(connectionFactory), "Person" + _personCounter++, Gender.Male);
            return new DatabasePersonRepository(connectionFactory).Create(person).Id;
        }

        private static int _roundCounter;
        public static int GetRound(IDbConnectionFactory connectionFactory)
        {
            var count = _roundCounter++;
            var round = new Round(0, null, "Category" + count, "Name" + count, true, new[] { new RoundTarget(0, ScoringType.Imperial, new Length(2, LengthUnit.Centimeters), new Length(1.5m, LengthUnit.Yards), 36) });

            return new DatabaseRoundRepository(connectionFactory).Create(round).Id;
        }
    }
}