using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewFiledAtRatingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Ratings");
        }
    }
}
