using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CelebrityPostCache3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CeleebrityPostCache",
                table: "CeleebrityPostCache");

            migrationBuilder.RenameTable(
                name: "CeleebrityPostCache",
                newName: "CelebrityPostCache");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CelebrityPostCache",
                table: "CelebrityPostCache",
                column: "CelebrityPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CelebrityPostCache",
                table: "CelebrityPostCache");

            migrationBuilder.RenameTable(
                name: "CelebrityPostCache",
                newName: "CeleebrityPostCache");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CeleebrityPostCache",
                table: "CeleebrityPostCache",
                column: "CelebrityPostId");
        }
    }
}
