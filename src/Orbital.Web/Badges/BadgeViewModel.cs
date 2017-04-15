namespace Orbital.Web.Badges
{
    public class BadgeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public string Algorithm { get; set; }

        public bool Multiple { get; set; }

        public string ImageUrl { get; set; }
    }
}
