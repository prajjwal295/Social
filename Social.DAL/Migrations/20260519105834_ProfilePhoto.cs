using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BasicInfo_ProfilePicturePublicId",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BasicInfo_ProfilePicutreUrl",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicInfo_ProfilePicturePublicId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "BasicInfo_ProfilePicutreUrl",
                table: "UserProfiles");
        }
    }
}
