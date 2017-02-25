namespace Orbital.Models.Domain
{
    public class Club
    {
        public Club(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Club;
            if (other == null) return false;

            return other.Id == Id && other.Name == Name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Id * 317 + Name.GetHashCode();
            }
        }
    }
}
