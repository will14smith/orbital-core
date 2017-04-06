using System.Collections.Generic;

namespace Orbital.Web.Rounds
{
    public interface IRoundService
    {
        IReadOnlyCollection<RoundViewModel> GetAll();
        IReadOnlyCollection<RoundViewModel> GetAllByVariant(int parentId);
        RoundViewModel GetById(int id);

        RoundViewModel Create(RoundInputModel input);
        RoundViewModel Update(RoundInputModel input);
        bool Delete(int id);

    }
}