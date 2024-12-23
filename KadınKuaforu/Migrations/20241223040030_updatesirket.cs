using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KadınKuaforu.Migrations
{
    /// <inheritdoc />
    public partial class updatesirket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "CloseTime",
                table: "Companies",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpenTime",
                table: "Companies",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "Companies");
        }
    }
}
