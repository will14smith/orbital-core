using System;
using System.Linq;
using System.Linq.Expressions;
using Orbital.Data;
using Orbital.Versioning;
using Orbital.Web.Models;

namespace Orbital.Web.Helpers
{
    public static class VersioningExtensions
    {
        public static VersionInfo GetVersionInfo<TEntity>(this OrbitalContext ctx, Guid id)
            where TEntity : IEntity
        {
            return GetVersionInfo<TEntity>(ctx, x => x.Id == id);
        }

        public static VersionInfo GetVersionInfo<TEntity>(this OrbitalContext ctx, Expression<Func<TEntity, bool>> selector)
            where TEntity : IEntity
        {
            // TODO select a better subset of data
            var versions = ctx.GetAllVersions(selector).OrderBy(x => x.Date).ToList();

            var created = GetVersionInfoEvent(versions.First());

            var count = versions.Count;
            if (count == 1)
            {
                return new VersionInfo(created, null, null);
            }

            VersionInfoEvent modified;

            var version = versions[count - 1];
            if (!version.Entity.Deleted)
            {
                modified = GetVersionInfoEvent(version);
                return new VersionInfo(created, modified, null);
            }

            var deleted = GetVersionInfoEvent(version);
            if (count <= 2)
            {
                return new VersionInfo(created, null, deleted);
            }

            modified = GetVersionInfoEvent(versions[count - 2]);
            return new VersionInfo(created, modified, deleted);
        }

        private static VersionInfoEvent GetVersionInfoEvent<TEntity>(Version<TEntity> version)
        {
            var by = version.GetUserMetadata().UserId;
            var on = version.Date;

            return new VersionInfoEvent(by, on);
        }
    }
}
