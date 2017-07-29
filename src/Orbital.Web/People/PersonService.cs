using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orbital.Data;
using Orbital.Data.Entities;
using Orbital.Models;
using Orbital.Models.Extensions;
using Orbital.Web.Clubs;
using Orbital.Web.Helpers;

namespace Orbital.Web.People
{
    public class PersonService : IPersonService
    {
        private readonly OrbitalContext _ctx;
        private readonly IClubService _clubService;

        public PersonService(OrbitalContext ctx, IClubService clubService)
        {
            _ctx = ctx;
            _clubService = clubService;
        }

        public async Task<IReadOnlyCollection<Person>> GetAll()
        {
            var people = await _ctx.People.Where(x => !x.Deleted).ToListAsync();

            return people.Select(ToDomain).ToList();
        }

        public async Task<PersonViewModel> GetById(Guid id)
        {
            var entity = await Find(id);
            if (entity == null)
            {
                return null;
            }

            var domain = ToDomain(entity);
            // TODO handle this being null
            var club = await _clubService.GetById(entity.ClubId);
            var versionInfo = await _ctx.GetVersionInfo<PersonEntity>(id);

            return new PersonViewModel(domain, club.Club, versionInfo);
        }

        public async Task<Guid> Create(PersonInputModel person)
        {
            var entity = new PersonEntity { Id = Guid.NewGuid() };
            PopulateEntity(entity, person);

            await _ctx.People.AddAsync(entity);
            await _ctx.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Update(Guid id, PersonInputModel person)
        {
            var entity = await Find(id);
            if (entity == null)
            {
                throw new Exception($"Couldn't find person with id = {id} to update");
            }

            PopulateEntity(entity, person);

            _ctx.People.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await Find(id);
            if (entity == null)
            {
                throw new Exception($"Couldn't find person with id = {id} to delete");
            }

            entity.Deleted = true;

            _ctx.People.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        private async Task<PersonEntity> Find(Guid id)
        {
            var entity = await _ctx.People.FindAsync(id);
            if (entity == null || entity.Deleted)
            {
                return null;
            }

            return entity;
        }

        private static Person ToDomain(PersonEntity entity)
        {
            return new Person(
                entity.Id, entity.ClubId,
                entity.Name,
                (Gender) entity.Gender, (Bowstyle?) entity.Bowstyle, 
                entity.ArcheryGBNumber,
                entity.DateOfBirth.ToLocalDate(), entity.DateStartedArchery.ToLocalDate());
        }

        private static void PopulateEntity(PersonEntity entity, PersonInputModel person)
        {
            entity.ClubId = person.ClubId;
            
            entity.Name = person.Name;
            
            entity.Gender = (int) person.Gender;
            entity.Bowstyle = (int?) person.Bowstyle;
            entity.ArcheryGBNumber = person.ArcheryGBNumber;
            entity.DateOfBirth = NormaliseDate(person.DateOfBirth);
            entity.DateStartedArchery = NormaliseDate(person.DateStartedArchery);
        }

        private static DateTime? NormaliseDate(DateTime? date)
        {
            return date.ToLocalDate().ToDateTime();
        }
    }
}
