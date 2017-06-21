using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Orbital.Data;

namespace Orbital.Data.Migrations
{
    [DbContext(typeof(OrbitalContext))]
    [Migration("20170618165913_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Autogen.Versioning.BadgeEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<string>("Field_Algorithm")
                        .HasColumnName("Field_Algorithm");

                    b.Property<string>("Field_Category")
                        .HasColumnName("Field_Category");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<string>("Field_Description")
                        .HasColumnName("Field_Description");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<string>("Field_ImageUrl")
                        .HasColumnName("Field_ImageUrl");

                    b.Property<bool>("Field_Multiple")
                        .HasColumnName("Field_Multiple");

                    b.Property<string>("Field_Name")
                        .HasColumnName("Field_Name");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("badge_history");
                });

            modelBuilder.Entity("Autogen.Versioning.BadgeHolderEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<DateTime>("Field_AwardedOn")
                        .HasColumnName("Field_AwardedOn");

                    b.Property<Guid>("Field_BadgeId")
                        .HasColumnName("Field_BadgeId");

                    b.Property<DateTime?>("Field_ConfirmedOn")
                        .HasColumnName("Field_ConfirmedOn");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<DateTime?>("Field_DeliveredOn")
                        .HasColumnName("Field_DeliveredOn");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<DateTime?>("Field_MadeOn")
                        .HasColumnName("Field_MadeOn");

                    b.Property<Guid>("Field_PersonId")
                        .HasColumnName("Field_PersonId");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("badge_holder_history");
                });

            modelBuilder.Entity("Autogen.Versioning.ClubEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<string>("Field_Name")
                        .HasColumnName("Field_Name");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("club_history");
                });

            modelBuilder.Entity("Autogen.Versioning.CompetitionEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<DateTimeOffset>("Field_End")
                        .HasColumnName("Field_End");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<string>("Field_Name")
                        .HasColumnName("Field_Name");

                    b.Property<DateTimeOffset>("Field_Start")
                        .HasColumnName("Field_Start");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("competition_history");
                });

            modelBuilder.Entity("Autogen.Versioning.CompetitionRoundEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<Guid>("Field_CompetitionId")
                        .HasColumnName("Field_CompetitionId");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<Guid>("Field_RoundId")
                        .HasColumnName("Field_RoundId");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("competition_round_history");
                });

            modelBuilder.Entity("Autogen.Versioning.HandicapEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<int>("Field_Bowstyle")
                        .HasColumnName("Field_Bowstyle");

                    b.Property<DateTime>("Field_Date")
                        .HasColumnName("Field_Date");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<bool>("Field_Indoor")
                        .HasColumnName("Field_Indoor");

                    b.Property<Guid>("Field_PersonId")
                        .HasColumnName("Field_PersonId");

                    b.Property<Guid?>("Field_ScoreId")
                        .HasColumnName("Field_ScoreId");

                    b.Property<int>("Field_Type")
                        .HasColumnName("Field_Type");

                    b.Property<int>("Field_Value")
                        .HasColumnName("Field_Value");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("handicap_history");
                });

            modelBuilder.Entity("Autogen.Versioning.PersonEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<string>("Field_ArcheryGBNumber")
                        .HasColumnName("Field_ArcheryGBNumber");

                    b.Property<int?>("Field_Bowstyle")
                        .HasColumnName("Field_Bowstyle");

                    b.Property<Guid>("Field_ClubId")
                        .HasColumnName("Field_ClubId");

                    b.Property<DateTime?>("Field_DateOfBirth")
                        .HasColumnName("Field_DateOfBirth");

                    b.Property<DateTime?>("Field_DateStartedArchery")
                        .HasColumnName("Field_DateStartedArchery");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<int>("Field_Gender")
                        .HasColumnName("Field_Gender");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<string>("Field_Name")
                        .HasColumnName("Field_Name");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("person_history");
                });

            modelBuilder.Entity("Autogen.Versioning.RoundEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<string>("Field_Category")
                        .HasColumnName("Field_Category");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<bool>("Field_Indoor")
                        .HasColumnName("Field_Indoor");

                    b.Property<string>("Field_Name")
                        .HasColumnName("Field_Name");

                    b.Property<Guid?>("Field_VariantOfId")
                        .HasColumnName("Field_VariantOfId");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("round_history");
                });

            modelBuilder.Entity("Autogen.Versioning.RoundTargetEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<int>("Field_ArrowCount")
                        .HasColumnName("Field_ArrowCount");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<int>("Field_DistanceUnit")
                        .HasColumnName("Field_DistanceUnit");

                    b.Property<decimal>("Field_DistanceValue")
                        .HasColumnName("Field_DistanceValue");

                    b.Property<int>("Field_FaceSizeUnit")
                        .HasColumnName("Field_FaceSizeUnit");

                    b.Property<decimal>("Field_FaceSizeValue")
                        .HasColumnName("Field_FaceSizeValue");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<Guid>("Field_RoundId")
                        .HasColumnName("Field_RoundId");

                    b.Property<int>("Field_ScoringType")
                        .HasColumnName("Field_ScoringType");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("round_target_history");
                });

            modelBuilder.Entity("Autogen.Versioning.ScoreEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<int>("Field_Bowstyle")
                        .HasColumnName("Field_Bowstyle");

                    b.Property<Guid>("Field_ClubId")
                        .HasColumnName("Field_ClubId");

                    b.Property<Guid?>("Field_CompetitionId")
                        .HasColumnName("Field_CompetitionId");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<DateTime>("Field_EnteredAt")
                        .HasColumnName("Field_EnteredAt");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<Guid>("Field_PersonId")
                        .HasColumnName("Field_PersonId");

                    b.Property<Guid>("Field_RoundId")
                        .HasColumnName("Field_RoundId");

                    b.Property<DateTime>("Field_ShotAt")
                        .HasColumnName("Field_ShotAt");

                    b.Property<decimal>("Field_TotalGolds")
                        .HasColumnName("Field_TotalGolds");

                    b.Property<decimal>("Field_TotalHits")
                        .HasColumnName("Field_TotalHits");

                    b.Property<decimal>("Field_TotalScore")
                        .HasColumnName("Field_TotalScore");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("score_history");
                });

            modelBuilder.Entity("Autogen.Versioning.ScoreTargetEntityVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<bool>("Field_Deleted")
                        .HasColumnName("Field_Deleted");

                    b.Property<decimal>("Field_Golds")
                        .HasColumnName("Field_Golds");

                    b.Property<decimal>("Field_Hits")
                        .HasColumnName("Field_Hits");

                    b.Property<Guid>("Field_Id")
                        .HasColumnName("Field_Id");

                    b.Property<Guid>("Field_ScoreId")
                        .HasColumnName("Field_ScoreId");

                    b.Property<decimal>("Field_ScoreValue")
                        .HasColumnName("Field_ScoreValue");

                    b.Property<Guid>("Metadata_UserMetadata_UserId")
                        .HasColumnName("Metadata_UserMetadata_UserId");

                    b.HasKey("Id");

                    b.ToTable("score_target_history");
                });

            modelBuilder.Entity("Orbital.Data.Entities.BadgeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Algorithm");

                    b.Property<string>("Category");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("Multiple");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("badge");
                });

            modelBuilder.Entity("Orbital.Data.Entities.BadgeHolderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AwardedOn");

                    b.Property<Guid>("BadgeId");

                    b.Property<DateTime?>("ConfirmedOn");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime?>("DeliveredOn");

                    b.Property<DateTime?>("MadeOn");

                    b.Property<Guid>("PersonId");

                    b.HasKey("Id");

                    b.HasIndex("BadgeId");

                    b.HasIndex("PersonId");

                    b.ToTable("badge_holder");
                });

            modelBuilder.Entity("Orbital.Data.Entities.ClubEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("club");
                });

            modelBuilder.Entity("Orbital.Data.Entities.CompetitionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Deleted");

                    b.Property<DateTimeOffset>("End");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("Start");

                    b.HasKey("Id");

                    b.ToTable("competition");
                });

            modelBuilder.Entity("Orbital.Data.Entities.CompetitionRoundEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CompetitionId");

                    b.Property<bool>("Deleted");

                    b.Property<Guid>("RoundId");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("RoundId");

                    b.ToTable("competition_round");
                });

            modelBuilder.Entity("Orbital.Data.Entities.HandicapEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Bowstyle");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Indoor");

                    b.Property<Guid>("PersonId");

                    b.Property<Guid?>("ScoreId");

                    b.Property<int>("Type");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("ScoreId");

                    b.ToTable("handicap");
                });

            modelBuilder.Entity("Orbital.Data.Entities.PersonEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArcheryGBNumber");

                    b.Property<int?>("Bowstyle");

                    b.Property<Guid>("ClubId");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<DateTime?>("DateStartedArchery");

                    b.Property<bool>("Deleted");

                    b.Property<int>("Gender");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("person");
                });

            modelBuilder.Entity("Orbital.Data.Entities.RoundEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Indoor");

                    b.Property<string>("Name");

                    b.Property<Guid?>("VariantOfId");

                    b.HasKey("Id");

                    b.HasIndex("VariantOfId");

                    b.ToTable("round");
                });

            modelBuilder.Entity("Orbital.Data.Entities.RoundTargetEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArrowCount");

                    b.Property<bool>("Deleted");

                    b.Property<int>("DistanceUnit");

                    b.Property<decimal>("DistanceValue");

                    b.Property<int>("FaceSizeUnit");

                    b.Property<decimal>("FaceSizeValue");

                    b.Property<Guid>("RoundId");

                    b.Property<int>("ScoringType");

                    b.HasKey("Id");

                    b.HasIndex("RoundId");

                    b.ToTable("round_target");
                });

            modelBuilder.Entity("Orbital.Data.Entities.ScoreEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Bowstyle");

                    b.Property<Guid>("ClubId");

                    b.Property<Guid?>("CompetitionId");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("EnteredAt");

                    b.Property<Guid>("PersonId");

                    b.Property<Guid>("RoundId");

                    b.Property<DateTime>("ShotAt");

                    b.Property<decimal>("TotalGolds");

                    b.Property<decimal>("TotalHits");

                    b.Property<decimal>("TotalScore");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("PersonId");

                    b.HasIndex("RoundId");

                    b.ToTable("score");
                });

            modelBuilder.Entity("Orbital.Data.Entities.ScoreTargetEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Deleted");

                    b.Property<decimal>("Golds");

                    b.Property<decimal>("Hits");

                    b.Property<Guid>("ScoreId");

                    b.Property<decimal>("ScoreValue")
                        .HasColumnName("Score");

                    b.HasKey("Id");

                    b.HasIndex("ScoreId");

                    b.ToTable("score_target");
                });

            modelBuilder.Entity("Orbital.Data.Entities.BadgeHolderEntity", b =>
                {
                    b.HasOne("Orbital.Data.Entities.BadgeEntity", "Badge")
                        .WithMany("BadgeHolders")
                        .HasForeignKey("BadgeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orbital.Data.Entities.PersonEntity", "Person")
                        .WithMany("HeldBadges")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orbital.Data.Entities.CompetitionRoundEntity", b =>
                {
                    b.HasOne("Orbital.Data.Entities.CompetitionEntity", "Competition")
                        .WithMany("Rounds")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orbital.Data.Entities.RoundEntity", "Round")
                        .WithMany("Competitions")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orbital.Data.Entities.HandicapEntity", b =>
                {
                    b.HasOne("Orbital.Data.Entities.PersonEntity", "Person")
                        .WithMany("Handicaps")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orbital.Data.Entities.ScoreEntity", "Score")
                        .WithMany("Handicaps")
                        .HasForeignKey("ScoreId");
                });

            modelBuilder.Entity("Orbital.Data.Entities.PersonEntity", b =>
                {
                    b.HasOne("Orbital.Data.Entities.ClubEntity", "Club")
                        .WithMany("Members")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orbital.Data.Entities.RoundEntity", b =>
                {
                    b.HasOne("Orbital.Data.Entities.RoundEntity", "ParentRound")
                        .WithMany("VariantRounds")
                        .HasForeignKey("VariantOfId");
                });

            modelBuilder.Entity("Orbital.Data.Entities.RoundTargetEntity", b =>
                {
                    b.HasOne("Orbital.Data.Entities.RoundEntity", "Round")
                        .WithMany("Targets")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orbital.Data.Entities.ScoreEntity", b =>
                {
                    b.HasOne("Orbital.Data.Entities.ClubEntity", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orbital.Data.Entities.CompetitionEntity", "Competition")
                        .WithMany("Scores")
                        .HasForeignKey("CompetitionId");

                    b.HasOne("Orbital.Data.Entities.PersonEntity", "Person")
                        .WithMany("Scores")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orbital.Data.Entities.RoundEntity", "Round")
                        .WithMany("Scores")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orbital.Data.Entities.ScoreTargetEntity", b =>
                {
                    b.HasOne("Orbital.Data.Entities.ScoreEntity", "Score")
                        .WithMany("Targets")
                        .HasForeignKey("ScoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
