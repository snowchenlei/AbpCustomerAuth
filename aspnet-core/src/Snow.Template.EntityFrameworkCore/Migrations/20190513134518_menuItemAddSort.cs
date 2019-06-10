using Microsoft.EntityFrameworkCore.Migrations;

namespace Snow.Template.Migrations
{
    public partial class menuItemAddSort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "MenuItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sort",
                table: "MenuItem");
        }
    }
}

