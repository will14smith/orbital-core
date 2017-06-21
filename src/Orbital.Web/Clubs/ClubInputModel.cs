using Orbital.Models.Domain;

namespace Orbital.Web.Clubs
{
    public class ClubInputModel
    {
        public ClubInputModel()
        {
        }
        public ClubInputModel(Club club)
        {
            Name = club.Name;
        }

        public string Name { get; set; }
    }
}