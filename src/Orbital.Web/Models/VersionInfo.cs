using System;

namespace Orbital.Web.Models
{
    public class VersionInfo
    {
        public VersionInfo(VersionInfoEvent created, VersionInfoEvent modified, VersionInfoEvent deleted)
        {
            Created = created ?? throw new ArgumentNullException(nameof(created));
            Modified = modified;
            Deleted = deleted;
        }

        public VersionInfoEvent Created { get; }
        public VersionInfoEvent Modified { get; }
        public VersionInfoEvent Deleted { get; }
    }
}