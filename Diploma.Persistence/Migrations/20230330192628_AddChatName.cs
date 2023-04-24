using Microsoft.EntityFrameworkCore.Migrations;

namespace Diploma.Persistence.Migrations
{
    public partial class AddChatName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "chats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "chats");
        }
    }
}
