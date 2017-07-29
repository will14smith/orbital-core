using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orbital.Models;

namespace Orbital.Web.People
{
    public interface IPersonService
    {
        Task<IReadOnlyCollection<Person>> GetAll();
        Task<PersonViewModel> GetById(Guid id);

        Task<Guid> Create(PersonInputModel club);
        Task Update(Guid id, PersonInputModel club);
        Task Delete(Guid id);
    }
}