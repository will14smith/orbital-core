using System;
using System.Collections.Generic;
using System.Linq;
using Orbital.Data;
using Orbital.Data.Entities;
using Orbital.Models.Domain;
using Orbital.Versioning;
using Orbital.Web.Helpers;
using Orbital.Web.Models;

namespace Orbital.Web.Clubs
{
    public class ClubService : IClubService
    {
        private readonly OrbitalContext _ctx;

        public ClubService(OrbitalContext ctx)
        {
            _ctx = ctx;
        }

        public IReadOnlyCollection<Club> GetAll()
        {
            return _ctx.Clubs
                .Where(x => !x.Deleted)
                .Select(ToDomain)
                .ToList();
        }

        public ClubViewModel GetById(Guid id)
        {
            var entity = Find(id);
            if (entity == null)
            {
                return null;
            }

            return new ClubViewModel(
                ToDomain(entity),
                GetVersionInfo(id)
            );
        }

        private VersionInfo GetVersionInfo(Guid id)
        {
            // TODO select a better subset of data
            var versions = _ctx.GetAllVersions<ClubEntity>(x => x.Id == id).OrderBy(x => x.Date).ToList();

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

        public Guid Create(ClubInputModel club)
        {
            var entity = new ClubEntity { Id = Guid.NewGuid() };
            PopulateEntity(entity, club);

            _ctx.Clubs.Add(entity);
            _ctx.SaveChanges();

            return entity.Id;
        }

        public void Update(Guid id, ClubInputModel club)
        {
            var entity = Find(id);
            if (entity == null)
            {
                throw new Exception($"Couldn't find club with id = {id} to update");
            }

            PopulateEntity(entity, club);

            _ctx.Clubs.Update(entity);
            _ctx.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = Find(id);
            if (entity == null)
            {
                throw new Exception($"Couldn't find club with id = {id} to update");
            }

            entity.Deleted = true;

            _ctx.Clubs.Update(entity);
            _ctx.SaveChanges();
        }

        private ClubEntity Find(Guid id)
        {
            var entity = _ctx.Clubs.Find(id);
            if (entity == null || entity.Deleted)
            {
                return null;
            }

            return entity;
        }

        private Club ToDomain(ClubEntity entity)
        {
            return new Club(entity.Id, entity.Name);
        }

        private void PopulateEntity(ClubEntity entity, ClubInputModel club)
        {
            entity.Name = club.Name;
        }
    }
}
