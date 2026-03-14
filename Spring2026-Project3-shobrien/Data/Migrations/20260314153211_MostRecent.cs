using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spring2026_Project3_shobrien.Data.Migrations
{
    /// <inheritdoc />
    public partial class MostRecent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterURL",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "PhotoURL",
                table: "Actors");

            migrationBuilder.AddColumn<byte[]>(
                name: "Poster",
                table: "Movies",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Actors",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poster",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Actors");

            migrationBuilder.AddColumn<string>(
                name: "PosterURL",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoURL",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
