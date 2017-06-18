using Microsoft.EntityFrameworkCore;
using Orbital.Data.Entities;
using Orbital.Versioning;

namespace Orbital.Data
{
    public class OrbitalContext : DbContext
    {
        public OrbitalContext(DbContextOptions<OrbitalContext> options)
            : base(options)
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

        public override int SaveChanges()
        {
            this.SyncVersioning();

            return base.SaveChanges();
        }
    }
}
