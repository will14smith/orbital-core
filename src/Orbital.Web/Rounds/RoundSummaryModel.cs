using System;
using Orbital.Models;

namespace Orbital.Web.Rounds
{
    public class RoundSummaryModel
    {
        public RoundSummaryModel(Round round)
        {
            Id = round.Id;
            Name = round.Name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}