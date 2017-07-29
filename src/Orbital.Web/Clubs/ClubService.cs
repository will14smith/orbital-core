using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IReadOnlyCollection<Club>> GetAll()
        {
            var clubs = await _ctx.Clubs.Where(x => !x.Deleted).ToListAsync();

            return clubs.Select(ToDomain).ToList();
        }

        public async Task<ClubViewModel> GetById(Guid id)
        {
            var entity = await Find(id);
            if (entity == null)
            {
                return null;
            }

            var domain = ToDomain(entity);
            var versionInfo = await _ctx.GetVersionInfo<ClubEntity>(id);

            return new ClubViewModel(domain, versionInfo);
        }

        public async Task<Guid> Create(ClubInputModel club)
        {
            var entity = new ClubEntity { Id = Guid.NewGuid() };
            PopulateEntity(entity, club);

            await _ctx.Clubs.AddAsync(entity);
            await _ctx.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Update(Guid id, ClubInputModel club)
        {
            var entity = await Find(id);
            if (entity == null)
            {
                throw new Exception($"Couldn't find club with id = {id} to update");
            }

            PopulateEntity(entity, club);

            _ctx.Clubs.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await Find(id);
            if (entity == null)
            {
                throw new Exception($"Couldn't find club with id = {id} to delete");
            }

            entity.Deleted = true;

            _ctx.Clubs.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        private async Task<ClubEntity> Find(Guid id)
        {
            var entity = await _ctx.Clubs.FindAsync(id);
            if (entity == null || entity.Deleted)
            {
                return null;
            }

            return entity;
        }

        private static Club ToDomain(ClubEntity entity)
        {
            return new Club(entity.Id, entity.Name);
        }

        private static void PopulateEntity(ClubEntity entity, ClubInputModel club)
        {
            entity.Name = club.Name;
        }
    }
}
