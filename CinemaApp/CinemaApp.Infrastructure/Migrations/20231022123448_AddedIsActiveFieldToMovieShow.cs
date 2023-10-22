using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsActiveFieldToMovieShow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MovieShows",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MovieShows");
        }
    }
}
