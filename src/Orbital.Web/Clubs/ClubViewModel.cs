using System;
using Orbital.Models.Domain;

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

    public class VersionInfo
    {
        public VersionInfo(VersionInfoAction created, VersionInfoAction modified, VersionInfoAction deleted)
        {
            Created = created ?? throw new ArgumentNullException(nameof(created));
            Modified = modified;
            Deleted = deleted;
        }

        public VersionInfoAction Created { get; }
        public VersionInfoAction Modified { get; }
        public VersionInfoAction Deleted { get; }
    }

    public class VersionInfoAction
    {
        public VersionInfoAction(Guid by, DateTime on)
        {
            By = by;
            On = on;
        }

        public Guid By { get; }
        public DateTime On { get; }
    }
}
