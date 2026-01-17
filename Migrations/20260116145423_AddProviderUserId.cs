using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bricouli.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProviderApplications",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProviderApplications");
        }
    }
}
