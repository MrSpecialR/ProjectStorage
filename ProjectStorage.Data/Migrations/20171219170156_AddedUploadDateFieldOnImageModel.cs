using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ProjectStorage.Data.Migrations
{
    public partial class AddedUploadDateFieldOnImageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "Images",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "Images");
        }
    }
}