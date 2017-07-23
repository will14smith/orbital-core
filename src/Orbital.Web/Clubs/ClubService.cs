using System;
using System.Collections.Generic;
using System.Linq;
using Orbital.Data;
using Orbital.Data.Entities;
using Orbital.Models.Domain;
using Orbital.Web.Helpers;

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
                .AsEnumerable()
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
                _ctx.GetVersionInfo<ClubEntity>(id)
            );
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
