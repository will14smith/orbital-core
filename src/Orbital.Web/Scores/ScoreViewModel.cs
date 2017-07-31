using Orbital.Models;
using Orbital.Web.Models;

namespace Orbital.Web.Scores
{
    public class ScoreViewModel
    {
        public ScoreViewModel(Score score, VersionInfo versioning)
        {
            Score = score;
            Versioning = versioning;
        }

        public Score Score { get; }

        public VersionInfo Versioning { get; }
    }
}