using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetroNetwork.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStationDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stations");
        }
    }
}
