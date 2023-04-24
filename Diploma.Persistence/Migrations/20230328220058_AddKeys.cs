using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diploma.Persistence.Migrations
{
    public partial class AddKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_private_keys",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_private_keys", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_private_keys_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_public_keys",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    x = table.Column<string>(type: "text", nullable: false),
                    y = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_public_keys", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_public_keys_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_private_keys_user_id",
                table: "user_private_keys",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_public_keys_user_id",
                table: "user_public_keys",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_private_keys");

            migrationBuilder.DropTable(
                name: "user_public_keys");
        }
    }
}
