using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin_AI.Migrations
{
    /// <inheritdoc />
    public partial class updatefled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phoneno",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phoneno",
                table: "Users");
        }
    }
}
