namespace Orbital.Models.Domain
{
    public class Length
    {
        public Length(decimal value, LengthUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public decimal Value { get; private set; }
        public LengthUnit Unit { get; private set; }
    }
}
