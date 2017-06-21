using System;

namespace Orbital.Web.Models
{
    public class VersionInfoEvent
    {
        public VersionInfoEvent(Guid by, DateTime on)
        {
            By = by;
            On = on;
        }

        public Guid By { get; }
        public DateTime On { get; }
    }
}