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
    }
}
