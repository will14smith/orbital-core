using Microsoft.EntityFrameworkCore;

namespace Orbital.Versioning
{
    public class VersionDbContextOptionsBuilder
    {
        private readonly DbContextOptionsBuilder _optionsBuilder;
        private readonly VersionExtension _extension;

        public VersionDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder, VersionExtension extension)
        {
            _optionsBuilder = optionsBuilder;
            _extension = extension;
        }

        public VersionDbContextOptionsBuilder WithMetadataProvider(IVersionMetadataProvider metadataProvider)
        {
            _extension.Metadata.Add(metadataProvider);
            return this;
        }
    }
}