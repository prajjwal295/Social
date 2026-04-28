using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CelebrityPostCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Followers",
                table: "Followers");

            migrationBuilder.RenameTable(
                name: "Followers",
                newName: "Follow");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FollowerId_FolloweeId",
                table: "Follow",
                newName: "IX_Follow_FollowerId_FolloweeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Follow",
                table: "Follow",
                column: "FollowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Follow",
                table: "Follow");

            migrationBuilder.RenameTable(
                name: "Follow",
                newName: "Followers");

            migrationBuilder.RenameIndex(
                name: "IX_Follow_FollowerId_FolloweeId",
                table: "Followers",
                newName: "IX_Followers_FollowerId_FolloweeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followers",
                table: "Followers",
                column: "FollowId");
        }
    }
}
