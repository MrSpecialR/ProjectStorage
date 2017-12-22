using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectStorage.Data.Migrations
{
    public partial class AddedPathToFolders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Folders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Folders");
        }
    }
}