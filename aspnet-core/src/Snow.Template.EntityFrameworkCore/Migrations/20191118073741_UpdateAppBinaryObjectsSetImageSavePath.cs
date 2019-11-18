using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Snow.Template.Migrations
{
    public partial class UpdateAppBinaryObjectsSetImageSavePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bytes",
                table: "AppBinaryObjects");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "AppBinaryObjects",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "AppBinaryObjects");

            migrationBuilder.AddColumn<byte[]>(
                name: "Bytes",
                table: "AppBinaryObjects",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
