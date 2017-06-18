using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Orbital.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "badge_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_Algorithm = table.Column<string>(nullable: true),
                    Field_Category = table.Column<string>(nullable: true),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_Description = table.Column<string>(nullable: true),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_ImageUrl = table.Column<string>(nullable: true),
                    Field_Multiple = table.Column<bool>(nullable: false),
                    Field_Name = table.Column<string>(nullable: true),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badge_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "badge_holder_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_AwardedOn = table.Column<DateTime>(nullable: false),
                    Field_BadgeId = table.Column<Guid>(nullable: false),
                    Field_ConfirmedOn = table.Column<DateTime>(nullable: true),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_DeliveredOn = table.Column<DateTime>(nullable: true),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_MadeOn = table.Column<DateTime>(nullable: true),
                    Field_PersonId = table.Column<Guid>(nullable: false),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badge_holder_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "club_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_Name = table.Column<string>(nullable: true),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_club_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "competition_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_End = table.Column<DateTimeOffset>(nullable: false),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_Name = table.Column<string>(nullable: true),
                    Field_Start = table.Column<DateTimeOffset>(nullable: false),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competition_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "competition_round_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_CompetitionId = table.Column<Guid>(nullable: false),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_RoundId = table.Column<Guid>(nullable: false),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competition_round_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "handicap_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_Bowstyle = table.Column<int>(nullable: false),
                    Field_Date = table.Column<DateTime>(nullable: false),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_Indoor = table.Column<bool>(nullable: false),
                    Field_PersonId = table.Column<Guid>(nullable: false),
                    Field_ScoreId = table.Column<Guid>(nullable: true),
                    Field_Type = table.Column<int>(nullable: false),
                    Field_Value = table.Column<int>(nullable: false),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_handicap_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "person_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_ArcheryGBNumber = table.Column<string>(nullable: true),
                    Field_Bowstyle = table.Column<int>(nullable: true),
                    Field_ClubId = table.Column<Guid>(nullable: false),
                    Field_DateOfBirth = table.Column<DateTime>(nullable: true),
                    Field_DateStartedArchery = table.Column<DateTime>(nullable: true),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_Gender = table.Column<int>(nullable: false),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_Name = table.Column<string>(nullable: true),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "round_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_Category = table.Column<string>(nullable: true),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_Indoor = table.Column<bool>(nullable: false),
                    Field_Name = table.Column<string>(nullable: true),
                    Field_VariantOfId = table.Column<Guid>(nullable: true),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "round_target_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_ArrowCount = table.Column<int>(nullable: false),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_DistanceUnit = table.Column<int>(nullable: false),
                    Field_DistanceValue = table.Column<decimal>(nullable: false),
                    Field_FaceSizeUnit = table.Column<int>(nullable: false),
                    Field_FaceSizeValue = table.Column<decimal>(nullable: false),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_RoundId = table.Column<Guid>(nullable: false),
                    Field_ScoringType = table.Column<int>(nullable: false),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round_target_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "score_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_Bowstyle = table.Column<int>(nullable: false),
                    Field_ClubId = table.Column<Guid>(nullable: false),
                    Field_CompetitionId = table.Column<Guid>(nullable: true),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_EnteredAt = table.Column<DateTime>(nullable: false),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_PersonId = table.Column<Guid>(nullable: false),
                    Field_RoundId = table.Column<Guid>(nullable: false),
                    Field_ShotAt = table.Column<DateTime>(nullable: false),
                    Field_TotalGolds = table.Column<decimal>(nullable: false),
                    Field_TotalHits = table.Column<decimal>(nullable: false),
                    Field_TotalScore = table.Column<decimal>(nullable: false),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_score_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "score_target_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Field_Deleted = table.Column<bool>(nullable: false),
                    Field_Golds = table.Column<decimal>(nullable: false),
                    Field_Hits = table.Column<decimal>(nullable: false),
                    Field_Id = table.Column<Guid>(nullable: false),
                    Field_ScoreId = table.Column<Guid>(nullable: false),
                    Field_ScoreValue = table.Column<decimal>(nullable: false),
                    Metadata_UserMetadata_UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_score_target_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "badge",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Algorithm = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Multiple = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "club",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_club", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "competition",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    End = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Start = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "round",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    Indoor = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    VariantOfId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round", x => x.Id);
                    table.ForeignKey(
                        name: "FK_round_round_VariantOfId",
                        column: x => x.VariantOfId,
                        principalTable: "round",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ArcheryGBNumber = table.Column<string>(nullable: true),
                    Bowstyle = table.Column<int>(nullable: true),
                    ClubId = table.Column<Guid>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    DateStartedArchery = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_person_club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "club",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "competition_round",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompetitionId = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competition_round", x => x.Id);
                    table.ForeignKey(
                        name: "FK_competition_round_competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_competition_round_round_RoundId",
                        column: x => x.RoundId,
                        principalTable: "round",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "round_target",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ArrowCount = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DistanceUnit = table.Column<int>(nullable: false),
                    DistanceValue = table.Column<decimal>(nullable: false),
                    FaceSizeUnit = table.Column<int>(nullable: false),
                    FaceSizeValue = table.Column<decimal>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false),
                    ScoringType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round_target", x => x.Id);
                    table.ForeignKey(
                        name: "FK_round_target_round_RoundId",
                        column: x => x.RoundId,
                        principalTable: "round",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "badge_holder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AwardedOn = table.Column<DateTime>(nullable: false),
                    BadgeId = table.Column<Guid>(nullable: false),
                    ConfirmedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    DeliveredOn = table.Column<DateTime>(nullable: true),
                    MadeOn = table.Column<DateTime>(nullable: true),
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badge_holder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_badge_holder_badge_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "badge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_badge_holder_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "score",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bowstyle = table.Column<int>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    CompetitionId = table.Column<Guid>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    EnteredAt = table.Column<DateTime>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false),
                    ShotAt = table.Column<DateTime>(nullable: false),
                    TotalGolds = table.Column<decimal>(nullable: false),
                    TotalHits = table.Column<decimal>(nullable: false),
                    TotalScore = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_score", x => x.Id);
                    table.ForeignKey(
                        name: "FK_score_club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "club",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_score_competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_score_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_score_round_RoundId",
                        column: x => x.RoundId,
                        principalTable: "round",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "handicap",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bowstyle = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Indoor = table.Column<bool>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false),
                    ScoreId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_handicap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_handicap_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_handicap_score_ScoreId",
                        column: x => x.ScoreId,
                        principalTable: "score",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "score_target",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Golds = table.Column<decimal>(nullable: false),
                    Hits = table.Column<decimal>(nullable: false),
                    ScoreId = table.Column<Guid>(nullable: false),
                    Score = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_score_target", x => x.Id);
                    table.ForeignKey(
                        name: "FK_score_target_score_ScoreId",
                        column: x => x.ScoreId,
                        principalTable: "score",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_badge_holder_BadgeId",
                table: "badge_holder",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_badge_holder_PersonId",
                table: "badge_holder",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_competition_round_CompetitionId",
                table: "competition_round",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_competition_round_RoundId",
                table: "competition_round",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_handicap_PersonId",
                table: "handicap",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_handicap_ScoreId",
                table: "handicap",
                column: "ScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_person_ClubId",
                table: "person",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_round_VariantOfId",
                table: "round",
                column: "VariantOfId");

            migrationBuilder.CreateIndex(
                name: "IX_round_target_RoundId",
                table: "round_target",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_score_ClubId",
                table: "score",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_score_CompetitionId",
                table: "score",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_score_PersonId",
                table: "score",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_score_RoundId",
                table: "score",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_score_target_ScoreId",
                table: "score_target",
                column: "ScoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "badge_history");

            migrationBuilder.DropTable(
                name: "badge_holder_history");

            migrationBuilder.DropTable(
                name: "club_history");

            migrationBuilder.DropTable(
                name: "competition_history");

            migrationBuilder.DropTable(
                name: "competition_round_history");

            migrationBuilder.DropTable(
                name: "handicap_history");

            migrationBuilder.DropTable(
                name: "person_history");

            migrationBuilder.DropTable(
                name: "round_history");

            migrationBuilder.DropTable(
                name: "round_target_history");

            migrationBuilder.DropTable(
                name: "score_history");

            migrationBuilder.DropTable(
                name: "score_target_history");

            migrationBuilder.DropTable(
                name: "badge_holder");

            migrationBuilder.DropTable(
                name: "competition_round");

            migrationBuilder.DropTable(
                name: "handicap");

            migrationBuilder.DropTable(
                name: "round_target");

            migrationBuilder.DropTable(
                name: "score_target");

            migrationBuilder.DropTable(
                name: "badge");

            migrationBuilder.DropTable(
                name: "score");

            migrationBuilder.DropTable(
                name: "competition");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "round");

            migrationBuilder.DropTable(
                name: "club");
        }
    }
}
