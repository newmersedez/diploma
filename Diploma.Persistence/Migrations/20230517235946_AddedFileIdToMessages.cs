using Microsoft.EntityFrameworkCore.Migrations;

namespace Diploma.Persistence.Migrations
{
    public partial class AddedFileIdToMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_attachments_attachment_id",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "attachment_id",
                table: "messages",
                newName: "file_id");

            migrationBuilder.RenameIndex(
                name: "IX_messages_attachment_id",
                table: "messages",
                newName: "IX_messages_file_id");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_files_file_id",
                table: "messages",
                column: "file_id",
                principalTable: "files",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_files_file_id",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "file_id",
                table: "messages",
                newName: "attachment_id");

            migrationBuilder.RenameIndex(
                name: "IX_messages_file_id",
                table: "messages",
                newName: "IX_messages_attachment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_attachments_attachment_id",
                table: "messages",
                column: "attachment_id",
                principalTable: "attachments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
