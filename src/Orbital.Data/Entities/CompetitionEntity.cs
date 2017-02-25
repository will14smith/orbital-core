﻿using System;

namespace Orbital.Data.Entities
{
    class CompetitionEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
