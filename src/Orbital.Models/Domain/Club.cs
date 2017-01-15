using System.Diagnostics.CodeAnalysis;

namespace Orbital.Models.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local", Justification = "Private setters are needed for serialisation")]
    public class Club
    {
        // needed for deserialisation
        public Club() { }

        public Club(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

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
                return Id * 317 + (Name.GetHashCode());
            }
        }
    }
}
