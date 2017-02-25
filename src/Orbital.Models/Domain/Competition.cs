using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbital.Models.Domain
{
    public class Competition
    {
        public Competition(int id, Competition competition)
            : this(
                  id: id,
                  name: competition.Name,
                  start: competition.Start,
                  end: competition.End,
                  rounds: competition.Rounds
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

        public int Id { get; }
        public string Name { get; }

        public DateTime Start { get; }
        public DateTime End { get; }

        public IReadOnlyCollection<int> Rounds { get; }

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
