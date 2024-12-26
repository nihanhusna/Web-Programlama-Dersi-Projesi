using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KadınKuaforu.Migrations
{
    /// <inheritdoc />
    public partial class addrendevular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identity_UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PersonnelId = table.Column<int>(type: "int", nullable: false),
                    RankTaskId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MeetStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    MeetFinish = table.Column<TimeSpan>(type: "time", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meetings_AspNetUsers_Identity_UserID",
                        column: x => x.Identity_UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Meetings_Personnels_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Meetings_RankTasks_RankTaskId",
                        column: x => x.RankTaskId,
                        principalTable: "RankTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_Identity_UserID",
                table: "Meetings",
                column: "Identity_UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_PersonnelId",
                table: "Meetings",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_RankTaskId",
                table: "Meetings",
                column: "RankTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meetings");
        }
    }
}
