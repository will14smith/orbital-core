using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Orbital.Data.Migrations
{
    public partial class Updatescoreandscoretarget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field_EnteredAt",
                table: "score_history");

            migrationBuilder.DropColumn(
                name: "EnteredAt",
                table: "score");

            migrationBuilder.AddColumn<Guid>(
                name: "Field_RoundTargetId",
                table: "score_target_history",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoundTargetId",
                table: "score_target",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_score_target_RoundTargetId",
                table: "score_target",
                column: "RoundTargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_score_target_round_target_RoundTargetId",
                table: "score_target",
                column: "RoundTargetId",
                principalTable: "round_target",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_score_target_round_target_RoundTargetId",
                table: "score_target");

            migrationBuilder.DropIndex(
                name: "IX_score_target_RoundTargetId",
                table: "score_target");

            migrationBuilder.DropColumn(
                name: "Field_RoundTargetId",
                table: "score_target_history");

            migrationBuilder.DropColumn(
                name: "RoundTargetId",
                table: "score_target");

            migrationBuilder.AddColumn<DateTime>(
                name: "Field_EnteredAt",
                table: "score_history",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EnteredAt",
                table: "score",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
