using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace Orbital.Versioning
{
    public class MetadataModel
    {
        public IVersionMetadataExtension MetadataExtension { get; }
        public IReadOnlyDictionary<PropertyInfo, PropertyDefinition> FieldMappings { get; }

        public MetadataModel(IVersionMetadataExtension metadataExtension, IReadOnlyDictionary<PropertyInfo, PropertyDefinition> fieldMappings)
        {
            MetadataExtension = metadataExtension;
            FieldMappings = fieldMappings;
        }
    }
}
