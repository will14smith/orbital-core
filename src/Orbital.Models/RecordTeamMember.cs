namespace Orbital.Models
{
    public class RecordTeamMember
    {
        public RecordTeamMember(int id, int personId, int? scoreId, int scoreValue)
        {
            Id = id;
            PersonId = personId;
            ScoreId = scoreId;
            ScoreValue = scoreValue;
        }

        public int Id { get; }

        public int PersonId { get; }

        // denormalise the score value incase the score isn't available (i.e. historical data)
        public int? ScoreId { get; }
        public int ScoreValue { get; }
    }
}