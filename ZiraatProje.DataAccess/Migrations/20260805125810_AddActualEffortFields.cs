using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZiraatProje.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddActualEffortFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AnalystActualManDays",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeveloperActualManDays",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalystActualManDays",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DeveloperActualManDays",
                table: "Projects");
        }
    }
}
