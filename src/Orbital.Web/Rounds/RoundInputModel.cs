using System;
using System.Collections.Generic;
using Orbital.Models;

namespace Orbital.Web.Rounds
{
    public class RoundInputModel
    {
        public RoundInputModel()
        {
        }
        public RoundInputModel(Round round)
        {
            throw new NotImplementedException();
        }

        public Guid? VariantOfId { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }

        public bool Indoor { get; set; }

        public List<RoundTargetInputModel> Targets { get; set; }
    }
}