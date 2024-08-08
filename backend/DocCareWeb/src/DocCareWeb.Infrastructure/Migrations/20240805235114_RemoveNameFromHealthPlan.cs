using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocCareWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNameFromHealthPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "HealthPlans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HealthPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
