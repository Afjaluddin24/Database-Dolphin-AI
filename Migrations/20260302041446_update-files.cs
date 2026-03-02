using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin_AI.Migrations
{
    /// <inheritdoc />
    public partial class updatefiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedUsers_Chats_ChatsId",
                table: "BlockedUsers");

            migrationBuilder.DropIndex(
                name: "IX_BlockedUsers_ChatsId",
                table: "BlockedUsers");

            migrationBuilder.DropColumn(
                name: "ChatsId",
                table: "BlockedUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "BlockedUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "BlockedUsers");

            migrationBuilder.AddColumn<int>(
                name: "ChatsId",
                table: "BlockedUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BlockedUsers_ChatsId",
                table: "BlockedUsers",
                column: "ChatsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedUsers_Chats_ChatsId",
                table: "BlockedUsers",
                column: "ChatsId",
                principalTable: "Chats",
                principalColumn: "ChatsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
