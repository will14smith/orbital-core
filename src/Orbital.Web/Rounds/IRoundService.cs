using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orbital.Models;

namespace Orbital.Web.Rounds
{
    public interface IRoundService
    {
        Task<IReadOnlyCollection<Round>> GetAll();

        Task<RoundViewModel> GetById(Guid id);

        Task<Guid> Create(RoundInputModel input);
        Task Update(Guid id, RoundInputModel input);
        Task Delete(Guid id);
    }
}