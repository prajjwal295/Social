using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PostInteractionisUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "PostInteraction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "PostInteraction");
        }
    }
}
