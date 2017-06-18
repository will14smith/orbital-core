using System;

namespace Orbital.Models.Domain
{
    public class Club
    {
        public Club(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
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
                return Id.GetHashCode() * 317 + Name.GetHashCode();
            }
        }
    }
}
