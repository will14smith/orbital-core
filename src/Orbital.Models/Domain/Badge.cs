using System.Diagnostics.CodeAnalysis;

namespace Orbital.Models.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local", Justification = "Private setters are needed for serialisation")]
    public class Badge
    {
        // needed for deserialisation
        public Badge() { }

        public Badge(int id, Badge badge)
            : this(
                id: id,

                name: badge.Name,
                description: badge.Description,
                algorithm: badge.Algorithm,
                category: badge.Category,
                multiple: badge.Multiple,
                imageUrl: badge.ImageUrl
            )
        {
        }
        public Badge(int id, string name, string description, string algorithm, string category, bool multiple, string imageUrl)
        {
            Id = id;
            
            Name = name;
            Description = description;
            Algorithm = algorithm;
            Category = category;
            Multiple = multiple;
            ImageUrl = imageUrl;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Algorithm { get; private set; }
        public string Category { get; private set; }
        public bool Multiple { get; private set; }
        public string ImageUrl { get; private set; }
    }

}
