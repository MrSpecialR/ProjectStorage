using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProjectStorage.Data.Migrations
{
    public partial class AddedFolderInfoFieldsInProjectAndFolderModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RootFolderName",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FolderName",
                table: "Folders",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RootFolderName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FolderName",
                table: "Folders");
        }
    }
}
