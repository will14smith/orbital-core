using System;

namespace Orbital.Models.Domain
{
    public class Badge
    {
        public Badge(Guid id, Badge badge)
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
        public Badge(Guid id, string name, string description, string algorithm, string category, bool multiple, string imageUrl)
        {
            Id = id;

            Name = name;
            Description = description;
            Algorithm = algorithm;
            Category = category;
            Multiple = multiple;
            ImageUrl = imageUrl;
        }

        public Guid Id { get; }

        public string Name { get; }
        public string Description { get; }
        public string Algorithm { get; }
        public string Category { get; }
        public bool Multiple { get; }
        public string ImageUrl { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as Badge;
            return other != null && Equals(other);
        }

        protected bool Equals(Badge other)
        {
            return Id == other.Id
                && string.Equals(Name, other.Name)
                && string.Equals(Description, other.Description)
                && string.Equals(Algorithm, other.Algorithm)
                && string.Equals(Category, other.Category)
                && Multiple == other.Multiple
                && string.Equals(ImageUrl, other.ImageUrl);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Algorithm?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Category?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Multiple.GetHashCode();
                hashCode = (hashCode * 397) ^ (ImageUrl?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
