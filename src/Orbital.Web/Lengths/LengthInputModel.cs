using Orbital.Models.Domain;

namespace Orbital.Web.Lengths
{
    public class LengthInputModel
    {
        public decimal Value { get; set; }
        public LengthUnit Unit { get; set; }
    }
}