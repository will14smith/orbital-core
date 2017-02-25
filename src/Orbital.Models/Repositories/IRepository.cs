using System.Collections.Generic;

namespace Orbital.Models.Repositories
{
    public interface IRepository<TModel>
    {
        IReadOnlyCollection<TModel> GetAll();
        TModel GetById(int id);

        TModel Create(TModel club);
        TModel Update(TModel club);
        bool Delete(TModel club);
    }
}
