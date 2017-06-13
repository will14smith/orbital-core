using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Orbital.Data.Versioning
{
    public static partial class DbContextExtensions
    {
        private static readonly MethodInfo EnumerableWhere;
        private static readonly MethodInfo EnumerableToList;
        private static readonly MethodInfo EnumerableSelect;

        static DbContextExtensions()
        {
            EnumerableWhere = typeof(Enumerable).GetRuntimeMethods()
                .Where(x => x.Name == nameof(Enumerable.Where))
                .Where(x =>
                {
                    var parameters = x.GetParameters();
                    return parameters.Length == 2
                           && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                           && parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>);
                })
                .Single();

            EnumerableSelect = typeof(Enumerable).GetRuntimeMethods()
                .Where(x => x.Name == nameof(Enumerable.Select))
                .Where(x =>
                {
                    var parameters = x.GetParameters();
                    return parameters.Length == 2
                           && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                           && parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>);
                })
                .Single();

            EnumerableToList = typeof(Enumerable).GetRuntimeMethods()
                .Where(x => x.Name == nameof(Enumerable.ToList))
                .Where(x =>
                {
                    var parameters = x.GetParameters();
                    return parameters.Length == 1
                           && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                })
                .Single();
        }

        internal static IReadOnlyDictionary<string, VersionEntityMapping> GetVersionEntityMappings(this DbContext context)
        {
            var annotation = context.Model.FindAnnotation(VersionModelCustomizer.ModelMappingAnnotation);

            var versionEntityMappings = annotation?.Value as IReadOnlyDictionary<string, VersionEntityMapping>;
            if (versionEntityMappings == null)
            {
                throw new InvalidOperationException($"Couldn't find version entity mappings, make sure the {nameof(VersionExtension)} is loaded.");
            }

            return versionEntityMappings;
        }
    }
}
