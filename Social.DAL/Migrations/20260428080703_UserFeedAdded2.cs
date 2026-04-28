using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserFeedAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFeed",
                columns: table => new
                {
                    UserFeedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeed", x => x.UserFeedId);
                });

            migrationBuilder.CreateTable(
                name: "UserFeedItems",
                columns: table => new
                {
                    UserFeedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeedItems", x => new { x.UserFeedId, x.PostId });
                    table.ForeignKey(
                        name: "FK_UserFeedItems_UserFeed_UserFeedId",
                        column: x => x.UserFeedId,
                        principalTable: "UserFeed",
                        principalColumn: "UserFeedId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFeedItems");

            migrationBuilder.DropTable(
                name: "UserFeed");
        }
    }
}
