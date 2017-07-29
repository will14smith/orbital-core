using System;

namespace Orbital.Versioning
{
    public class VersionDbContextOptionsBuilder
    {
        private readonly VersionExtension _extension;

        public VersionDbContextOptionsBuilder(VersionExtension extension)
        {
            _extension = extension;
        }

        public VersionDbContextOptionsBuilder WithMetadataExtension(IVersionMetadataExtension metadataExtension)
        {
            _extension.Metadata.Add(metadataExtension);
            return this;
        }
    }
}