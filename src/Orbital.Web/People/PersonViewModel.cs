using Orbital.Models;
using Orbital.Web.Models;

namespace Orbital.Web.People
{
    public class PersonViewModel
    {
        public PersonViewModel(Person person, Club club, VersionInfo versioning)
        {
            Person = person;
            Club = club;
            Versioning = versioning;
        }

        public Person Person { get; }
        public Club Club { get; }
        public VersionInfo Versioning { get; }
    }
}
