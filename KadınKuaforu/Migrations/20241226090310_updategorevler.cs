using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KadınKuaforu.Migrations
{
    /// <inheritdoc />
    public partial class updategorevler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "RankTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "RankTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "RankTasks");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "RankTasks");
        }
    }
}
