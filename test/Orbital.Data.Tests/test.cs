using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Orbital.Data.Entities;
using Orbital.Versioning;
using Xunit;

namespace Orbital.Data.Tests
{
    public class Test
    {
        [Fact]
        public void Test1()
        {
            var builder = new DbContextOptionsBuilder<OrbitalContext>();

            builder.UseVersioning();

            var context = new OrbitalContext(builder.Options);

            var clubEntity = new ClubEntity { Id = Guid.NewGuid(), Name = "ABC" };

            context.Clubs.Add(clubEntity);
            context.SaveChanges();

            clubEntity.Name = "DEF";
            context.Clubs.Update(clubEntity);
            context.SaveChanges();

            var versions = context.GetAllVersions<ClubEntity>(x => x.Id == clubEntity.Id).ToList();
        }
    }
}
