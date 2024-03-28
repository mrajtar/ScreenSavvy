using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenSavvy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathColumnForMoviePosters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "MovieDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "MovieDetails");
        }
    }
}
