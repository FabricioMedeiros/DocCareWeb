using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocCareWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNameFromSpecialty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Specialties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Specialties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
