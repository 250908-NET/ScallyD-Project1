using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoTournamentPlanner.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_Given = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_IsGivenFirst = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Ruleset = table.Column<int>(type: "int", nullable: false),
                    OrganizerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_Persons_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentParticipants",
                columns: table => new
                {
                    ParticipantsId = table.Column<int>(type: "int", nullable: false),
                    ParticipatingInId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentParticipants", x => new { x.ParticipantsId, x.ParticipatingInId });
                    table.ForeignKey(
                        name: "FK_TournamentParticipants_Persons_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentParticipants_Tournaments_ParticipatingInId",
                        column: x => x.ParticipatingInId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipants_ParticipatingInId",
                table: "TournamentParticipants",
                column: "ParticipatingInId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_OrganizerId",
                table: "Tournaments",
                column: "OrganizerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentParticipants");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
