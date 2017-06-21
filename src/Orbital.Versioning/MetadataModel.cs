using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace Orbital.Versioning
{
    public class MetadataModel
    {
        public IVersionMetadataProvider MetadataProvider { get; }
        public IReadOnlyDictionary<PropertyInfo, PropertyDefinition> FieldMappings { get; }

        public MetadataModel(IVersionMetadataProvider metadataProvider, IReadOnlyDictionary<PropertyInfo, PropertyDefinition> fieldMappings)
        {
            MetadataProvider = metadataProvider;
            FieldMappings = fieldMappings;
        }
    }
}
