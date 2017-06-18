using System.Diagnostics.CodeAnalysis;

namespace Orbital.Models.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local", Justification = "Private setters are needed for serialisation")]
    public class Length
    {
        // needed for deserialisation
        public Length() { }

        public Length(decimal value, LengthUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public decimal Value { get; private set; }
        public LengthUnit Unit { get; private set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as Length;
            return other != null && Equals(other);
        }

        protected bool Equals(Length other)
        {
            return Value == other.Value && Unit == other.Unit;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value.GetHashCode() * 397) ^ (int)Unit;
            }
        }
    }
}
