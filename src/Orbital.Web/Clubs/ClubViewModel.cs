using Orbital.Models.Domain;
using Orbital.Web.Models;

namespace Orbital.Web.Clubs
{
    public class ClubViewModel
    {
        public ClubViewModel(Club club, VersionInfo versioning)
        {
            Club = club;
            Versioning = versioning;
        }

        public Club Club { get; }
        public VersionInfo Versioning { get; }
    }
}
