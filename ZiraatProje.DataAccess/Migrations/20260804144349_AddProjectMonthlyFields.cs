using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZiraatProje.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectMonthlyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AnalistAy1",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AnalistAy2",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AnalistAy3",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "YazilimciAy1",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "YazilimciAy2",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "YazilimciAy3",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalistAy1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AnalistAy2",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AnalistAy3",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "YazilimciAy1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "YazilimciAy2",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "YazilimciAy3",
                table: "Projects");
        }
    }
}
