using System;
using System.Collections.Generic;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Schema.People
{
  public class PersonServiceImpl : IPersonService
  {
    private IPersonRepository _personRepository;

    public PersonServiceImpl(IPersonRepository personRepository)
    {
      _personRepository = personRepository;
    }

    public IReadOnlyCollection<Person> GetRoot()
    {
      return _personRepository.GetAll();
    }

    public IReadOnlyCollection<Person> GetByClub(Club club)
    {
      return _personRepository.GetAllByClubId(club.Id);
    }
  }
}