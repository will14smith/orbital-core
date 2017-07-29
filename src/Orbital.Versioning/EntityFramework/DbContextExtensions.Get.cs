using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Orbital.Versioning
{
    public static partial class DbContextExtensions
    {
        public static Task<IEnumerable<Version<T>>> GetAllVersions<T>(this DbContext context, Expression<Func<T, bool>> predicate)
        {
            var versionEntityMappings = context.GetVersionModels();
            if (!versionEntityMappings.TryGetValue(typeof(T).FullName, out var versionEntityMapping))
            {
                throw new InvalidOperationException($"Couldn't find entity mapping for {typeof(T).Name}");
            }

            var setMethod = typeof(DbContext).GetRuntimeMethod("Set", Type.EmptyTypes);
            var set = setMethod.MakeGenericMethod(versionEntityMapping.VersionType).Invoke(context, new object[0]);

            return Task.Run(() => TransformAndCompile(versionEntityMapping, predicate)(set));
        }

        private static Func<object, IEnumerable<Version<TEntity>>> TransformAndCompile<TEntity>(
            VersionModel versionModel, Expression<Func<TEntity, bool>> originalPredicate)
        {
            var predicate = Transform(versionModel, originalPredicate);
            var projection = BuildProjection<TEntity>(versionModel);

            return BuildMainLambda<TEntity>(predicate, projection, versionModel.VersionType).Compile();
        }

        private static Expression<Func<object, IEnumerable<Version<TEntity>>>> BuildMainLambda<TEntity>(
            Expression predicate, Expression projection, Type entityType)
        {
            // (object set) => ((DbSet<THistory>)set).Where(transformedPredicate).ToList().Select(ToVersion);
            var setParam = Expression.Parameter(typeof(object), "set");

            var set = Expression.Convert(setParam, typeof(DbSet<>).MakeGenericType(entityType));
            var where = Expression.Call(null, QueryableWhere.MakeGenericMethod(entityType), set, predicate);
            var toList = Expression.Call(null, EnumerableToList.MakeGenericMethod(entityType), where);
            var select = Expression.Call(null, EnumerableSelect.MakeGenericMethod(entityType, typeof(Version<TEntity>)), toList, projection);

            return Expression.Lambda<Func<object, IEnumerable<Version<TEntity>>>>(select, setParam);
        }

        private static LambdaExpression BuildProjection<TEntity>(VersionModel versionModel)
        {
            // (THistory x) => new Version<TEntity>((IVersionEntity<TEntity>)x);
            var entityParameter = Expression.Parameter(versionModel.VersionType, "entity");

            var entity = Expression.Convert(entityParameter, typeof(IVersionEntity<TEntity>));
            var body = Expression.New(typeof(Version<TEntity>).GetConstructor(typeof(IVersionEntity<TEntity>)), entity);

            return Expression.Lambda(body, entityParameter);
        }

        private static Expression Transform<T>(VersionModel versionModel, Expression<Func<T, bool>> predicate)
        {
            var visitor = new VersionExpressionMapper(versionModel);

            return visitor.Visit(predicate);
        }
    }
}
