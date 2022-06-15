using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DMovies.Migrations
{
    public partial class AddMovieDataAndImdbUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imdbMovieId",
                table: "MovieInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contentType",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "data",
                table: "Movie",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imdbMovieId",
                table: "MovieInfo");

            migrationBuilder.DropColumn(
                name: "contentType",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "data",
                table: "Movie");
        }
    }
}
