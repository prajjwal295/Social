using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken_Created",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "RefreshToken_Expires",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "RefreshToken_Revoked",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "RefreshToken_Token",
                table: "UserProfiles");

            migrationBuilder.AddColumn<int>(
                name: "RefreshToken_Capacity",
                table: "UserProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken_Capacity",
                table: "UserProfiles");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshToken_Created",
                table: "UserProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshToken_Expires",
                table: "UserProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshToken_Revoked",
                table: "UserProfiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken_Token",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
