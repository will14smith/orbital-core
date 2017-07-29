using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Orbital.Versioning.Tests.Integration
{
    public class DbContextIntegrationTests
    {
        public class Entity
        {
            public Guid Id { get; set; }

            public string Title { get; set; }
        }

        public class Context : DbContext
        {
            public Context(DbContextOptions options) : base(options) { }

            public DbSet<Entity> Entities { get; set; }
        }

        [Fact]
        public async void Test()
        {
            UserMetadataProvider.UserId = Guid.NewGuid();

            var builder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseVersioning(options =>
                {
                    options.WithMetadataExtension(new UserMetadataExtension());
                });

            var ctx = new Context(builder.Options);

            // insert
            var title = "EntityTitle";
            var entity = new Entity { Id = Guid.NewGuid(), Title = title };
            ctx.Entities.Add(entity);

            ctx.SyncVersioning();
            var rowsChanged = ctx.SaveChanges();
            Assert.Equal(2, rowsChanged);

            var lookedUpEntity = ctx.Entities.Find(entity.Id);
            Assert.NotNull(lookedUpEntity);
            Assert.Equal(entity.Title, lookedUpEntity.Title);

            var versions = await ctx.GetAllVersions<Entity>(x => x.Id == entity.Id);
            Assert.Equal(1, versions.Count());
            Assert.Equal(entity.Title, versions.First().Entity.Title);

            // update
            entity.Title = title + "-Updated";
            ctx.Entities.Update(entity);

            ctx.SyncVersioning();
            rowsChanged = ctx.SaveChanges();
            Assert.Equal(2, rowsChanged);

            lookedUpEntity = ctx.Entities.Find(entity.Id);
            Assert.NotNull(lookedUpEntity);
            Assert.Equal(entity.Title, lookedUpEntity.Title);

            versions = await ctx.GetAllVersions<Entity>(x => x.Id == entity.Id);
            versions = versions.OrderBy(x => x.VersionId).ToList();
            Assert.Equal(2, versions.Count());
            Assert.Equal(title, versions.First().Entity.Title);
            Assert.Equal(entity.Title, versions.Last().Entity.Title);

            // check metadata
            var user = versions.First().GetUserMetadata();
            Assert.NotNull(user);
            Assert.Equal(UserMetadataProvider.UserId, user.UserId);
        }

        [Fact]
        public async Task Test_NoSync()
        {
            var builder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseVersioning(options =>
                {
                    options.WithMetadataExtension(new UserMetadataExtension());
                });

            var ctx = new Context(builder.Options);

            // insert
            var title = "EntityTitle";
            var entity = new Entity { Id = Guid.NewGuid(), Title = title };
            ctx.Entities.Add(entity);

            var rowsChanged = ctx.SaveChanges();
            Assert.Equal(1, rowsChanged);

            var lookedUpEntity = ctx.Entities.Find(entity.Id);
            Assert.NotNull(lookedUpEntity);
            Assert.Equal(entity.Title, lookedUpEntity.Title);

            var versions = await ctx.GetAllVersions<Entity>(x => x.Id == entity.Id);
            Assert.Equal(0, versions.Count());

            // update
            entity.Title = title + "-Updated";
            ctx.Entities.Update(entity);

            rowsChanged = ctx.SaveChanges();
            Assert.Equal(1, rowsChanged);

            lookedUpEntity = ctx.Entities.Find(entity.Id);
            Assert.NotNull(lookedUpEntity);
            Assert.Equal(entity.Title, lookedUpEntity.Title);

            versions = await ctx.GetAllVersions<Entity>(x => x.Id == entity.Id);
            versions = versions.OrderBy(x => x.VersionId).ToList();
            Assert.Equal(0, versions.Count());
        }
    }
}
