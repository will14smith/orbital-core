using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Orbital.Models.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local", Justification = "Private setters are needed for serialisation")]
    public class Competition
    {
        // needed for deserialisation
        public Competition() { }

        public Competition(int id, Competition competition)
            : this(id, competition, competition.Rounds)
        {
        }

        public Competition(int id, Competition competition, IReadOnlyCollection<int> rounds)
            : this(
                  id: id,
                  name: competition.Name,
                  start: competition.Start,
                  end: competition.End,
                  rounds: rounds
            )
        {
        }

        public Competition(int id, string name, DateTime start, DateTime end, IReadOnlyCollection<int> rounds)
        {
            Id = id;
            Name = name;
            Start = start;
            End = end;
            Rounds = rounds;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public IReadOnlyCollection<int> Rounds { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as Competition;
            if (other == null) return false;

            return Id == other.Id
                   && Name == other.Name
                   && Start == other.Start
                   && End == other.End
                   && Rounds.SequenceEqual(other.Rounds);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode += Name.GetHashCode() * 317;
                hashCode += Start.GetHashCode() * 317;
                hashCode += End.GetHashCode() * 317;
                hashCode += Rounds?.GetHashCode() * 317 ?? 0;
                return hashCode;
            }
        }
    }
}
