using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orbital.Models;

namespace Orbital.Web.Scores
{
    public interface IScoreService
    {
        Task<IReadOnlyCollection<Score>> GetAll();

        Task<ScoreViewModel> GetById(Guid id);

        Task<Guid> Create(ScoreInputModel input);
        Task Update(Guid id, ScoreInputModel input);
        Task Delete(Guid id);
    }
}