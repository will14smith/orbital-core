using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Models.Services
{
    public interface IClubService
    {
        IReadOnlyCollection<Club> GetRoot();
        Club GetById(int id);

        Club Add(Club input);
        Club Update(int id, Club input);
    }
}