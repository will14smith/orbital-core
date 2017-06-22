using System.Collections.Generic;
using Orbital.Models.Domain;
using Orbital.Web.Models;

namespace Orbital.Web.Rounds
{
    public class RoundViewModel
    {
        public RoundViewModel(Round round, VersionInfo versioning)
        {
            Round = round;
            Targets = round.Targets;
            Versioning = versioning;
        }

        public Round Round { get; }
        public IReadOnlyCollection<RoundTarget> Targets { get; }

        public VersionInfo Versioning { get; }
    }
}