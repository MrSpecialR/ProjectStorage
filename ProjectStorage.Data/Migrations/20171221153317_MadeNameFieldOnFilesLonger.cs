using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectStorage.Data.Migrations
{
    public partial class MadeNameFieldOnFilesLonger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Files",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Files",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500);
        }
    }
}