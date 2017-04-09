using System.Collections.Generic;

namespace Orbital.Web.People
{
    public interface IPersonService
    {
        IReadOnlyCollection<PersonViewModel> GetAll();
        IReadOnlyCollection<PersonViewModel> GetAllByClubId(int clubId);
        PersonViewModel GetById(int id);


        PersonViewModel Create(PersonInputModel person);
        PersonViewModel Update(PersonInputModel person);
        bool Delete(int id);

    }
}