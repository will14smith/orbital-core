using System;
using System.Collections.Generic;
using System.Linq;
using Orbital.Data;
using Orbital.Data.Entities;
using Orbital.Models.Domain;

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
                .Select(x => new Club(x.Id, x.Name))
                .ToList();
        }

        public ClubViewModel GetById(Guid id)
        {
            throw new System.NotImplementedException();
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
            var entity = _ctx.Clubs.Find(id);
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
            throw new System.NotImplementedException();
        }

        private void PopulateEntity(ClubEntity entity, ClubInputModel club)
        {
            entity.Name = club.Name;
        }
    }
}
