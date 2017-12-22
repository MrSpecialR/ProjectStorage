using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ProjectStorage.Data.Migrations
{
    public partial class AddedLikedDateFieldOnUserFavouriteImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LikedDate",
                table: "UserFavouriteImages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedDate",
                table: "UserFavouriteImages");
        }
    }
}