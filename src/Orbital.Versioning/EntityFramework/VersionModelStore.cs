using System.Collections.Generic;

namespace Orbital.Versioning
{
    internal class VersionModelStore
    {
        public IReadOnlyDictionary<string, VersionModel> Models { get; set; }
    }
}