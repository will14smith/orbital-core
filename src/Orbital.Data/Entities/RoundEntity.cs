namespace Orbital.Data.Entities
{
    class RoundEntity
    {
        public int Id { get; set; }

        public int? VariantOfId { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public bool Indoor { get; set; }
    }
}
