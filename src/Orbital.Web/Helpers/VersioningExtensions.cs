using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Orbital.Data;
using Orbital.Versioning;
using Orbital.Web.Models;

namespace Orbital.Web.Helpers
{
    public static class VersioningExtensions
    {
        public static Task<VersionInfo> GetVersionInfo<TEntity>(this OrbitalContext ctx, Guid id)
            where TEntity : IEntity
        {
            return GetVersionInfo<TEntity>(ctx, x => x.Id == id);
        }

        public static async Task<VersionInfo> GetVersionInfo<TEntity>(this OrbitalContext ctx, Expression<Func<TEntity, bool>> selector)
            where TEntity : IEntity
        {
            // TODO select a better subset of data
            var versions = await ctx.GetAllVersions(selector);
            var orderedVersions = versions.OrderBy(x => x.Date).ToList();

            var count = orderedVersions.Count;
            if (count == 0)
            {
                return new VersionInfo(null, null, null);
            }


            var created = GetVersionInfoEvent(orderedVersions.FirstOrDefault());

            if (count == 1)
            {
                return new VersionInfo(created, null, null);
            }

            VersionInfoEvent modified;

            var version = orderedVersions[count - 1];
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

            modified = GetVersionInfoEvent(orderedVersions[count - 2]);
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
