using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Orbital.Data.Entities;

namespace Orbital.Data
{
    internal class OrbitalContext : DbContext
    {
        public OrbitalContext(DbContextOptions<OrbitalContext> options)
            : base(options.WithExtension(new Extension()))
        {
        }

        public DbSet<BadgeEntity> Badges { get; set; }
        public DbSet<BadgeHolderEntity> BadgeHolders { get; set; }
        public DbSet<ClubEntity> Clubs { get; set; }
        public DbSet<CompetitionEntity> Competitions { get; set; }
        public DbSet<CompetitionRoundEntity> CompetitionRounds { get; set; }
        public DbSet<HandicapEntity> Handicaps { get; set; }
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<RoundEntity> Rounds { get; set; }
        public DbSet<RoundTargetEntity> RoundTargets { get; set; }
        public DbSet<ScoreEntity> Scores { get; set; }
        public DbSet<ScoreTargetEntity> ScoreTargets { get; set; }
    }
}
