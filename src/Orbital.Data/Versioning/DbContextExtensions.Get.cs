using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Orbital.Data.Versioning
{
    public static partial class DbContextExtensions
    {
        public static IEnumerable<Version<T>> GetAllVersions<T>(this DbContext context, Expression<Func<T, bool>> predicate)
        {
            var versionEntityMappings = context.GetVersionEntityMappings();
            if (!versionEntityMappings.TryGetValue(typeof(T).Name, out var versionEntityMapping))
            {
                throw new InvalidOperationException($"Couldn't find entity mapping for {typeof(T).Name}");   
            }

            var setMethod = typeof(DbContext).GetRuntimeMethod("Set", Type.EmptyTypes);
            var set = setMethod.MakeGenericMethod(versionEntityMapping.VersionEntityType).Invoke(context, new object[0]);

            return TransformAndCompile(versionEntityMapping, predicate)(set);
        }

        private static Func<object, IEnumerable<Version<TEntity>>> TransformAndCompile<TEntity>(
            VersionEntityMapping versionEntityMapping, Expression<Func<TEntity, bool>> originalPredicate)
        {
            var predicate = Transform(versionEntityMapping, originalPredicate);
            var projection = BuildProjection<TEntity>(versionEntityMapping);

            return BuildMainLambda<TEntity>(predicate, projection, versionEntityMapping.VersionEntityType).Compile();
        }

        private static Expression<Func<object, IEnumerable<Version<TEntity>>>> BuildMainLambda<TEntity>(
            Expression predicate, Expression projection, Type entityType)
        {
            // TODO this isn't ideal, the ToList forces all the data to be loaded and then the filtering & projection is done in memory
            // This is because EF tries to use GetCustomAttributes on the predicate properties (from the dynamic types) and this is not supported.
            // Potential workaround is to replace the TypeBuilder code with Mono.Cecil to generate an actual assembly that can be loaded with all the features
            // Alternatively, wait for .NET Core 2.0 which should support AssemblyBuilderAccess.Save (i.e. what Mono.Cecil would be doing)

            // (object set) => ((DbSet<THistory>)set).ToList().Where(transformedPredicate).Select(ToVersion);
            var setParam = Expression.Parameter(typeof(object), "set");

            var set = Expression.Convert(setParam, typeof(DbSet<>).MakeGenericType(entityType));
            var toList = Expression.Call(null, EnumerableToList.MakeGenericMethod(entityType), set);
            var where = Expression.Call(null, EnumerableWhere.MakeGenericMethod(entityType), toList, predicate);
            var select = Expression.Call(null, EnumerableSelect.MakeGenericMethod(entityType, typeof(Version<TEntity>)), where, projection);

            return Expression.Lambda<Func<object, IEnumerable<Version<TEntity>>>>(select, setParam);
        }

        private static LambdaExpression BuildProjection<TEntity>(VersionEntityMapping versionEntityMapping)
        {
            // (THistory x) => new Version<TEntity>((IVersionEntity<TEntity>)x);
            var entityParameter = Expression.Parameter(versionEntityMapping.VersionEntityType, "entity");

            var entity = Expression.Convert(entityParameter, typeof(IVersionEntity<TEntity>));
            var body = Expression.New(typeof(Version<TEntity>).GetTypeInfo().GetConstructor(new[] { typeof(IVersionEntity<TEntity>) }), entity);

            return Expression.Lambda(body, entityParameter);
        }

        private static Expression Transform<T>(VersionEntityMapping versionEntityMapping, Expression<Func<T, bool>> predicate)
        {
            var visitor = new VersionEntityExpressionMapper(versionEntityMapping);

            return visitor.Visit(predicate);
        }

    }
}
