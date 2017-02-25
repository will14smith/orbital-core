using System.Collections.Generic;
using System.Linq;

namespace Orbital.Data.Repositories
{
    public abstract class InMemoryRepository<TModel>
    {
        protected readonly List<TModel> Data;

        protected InMemoryRepository()
        {
            Data = new List<TModel>();
        }

        protected InMemoryRepository(List<TModel> data)
        {
            Data = data;
        }

        public IReadOnlyCollection<TModel> GetAll() => Data;
        public TModel GetById(int id) => Data.FirstOrDefault(x => GetId(x) == id);

        public TModel Create(TModel item) { Data.Add(item); return item; }
        public TModel Update(TModel item) { Delete(item); return Create(item); }
        public bool Delete(TModel item) => Data.Remove(GetById(GetId(item)));

        protected abstract int GetId(TModel item);
    }
}
