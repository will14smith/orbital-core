using Orbital.Models.Domain;

namespace Orbital.Web.Lengths
{
    public static class LengthExtensions
    {
        public static LengthViewModel ToView(this Length length)
        {
            return new LengthViewModel
            {
                Value = length.Value,
                Unit = length.Unit
            };
        }

        public static Length FromInput(this LengthInputModel input)
        {
            return new Length(input.Value, input.Unit);
        }
    }
}
