using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CelebrityPostCache2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CeleebrityPostCache",
                columns: table => new
                {
                    CelebrityPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CeleebrityPostCache", x => x.CelebrityPostId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CeleebrityPostCache");

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
    }
}
