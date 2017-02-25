namespace Orbital.Models.Domain
{
    public class HandicapIdentifier
    {
        public HandicapIdentifier(bool indoor, Bowstyle bowstyle)
        {
            Indoor = indoor;
            Bowstyle = bowstyle;
        }

        public bool Indoor { get; }
        public Bowstyle Bowstyle { get; }
    }
}