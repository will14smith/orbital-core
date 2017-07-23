using System;
using Orbital.Data;
using Orbital.Data.Entities;
using Orbital.Models;

namespace Orbital.Web.Users
{
    internal class UserQuery : IUserQuery
    {
        private readonly OrbitalContext _ctx;

        public UserQuery(OrbitalContext ctx)
        {
            _ctx = ctx;
        }
        
        public User GetById(Guid id)
        {
            var person = Find(id);
            
            return new User(person.Id, person.Name);
        }

        private PersonEntity Find(Guid id)
        {
            var entity = _ctx.People.Find(id);
            if (entity == null || entity.Deleted)
            {
                return null;
            }

            return entity;
        }
    }
}