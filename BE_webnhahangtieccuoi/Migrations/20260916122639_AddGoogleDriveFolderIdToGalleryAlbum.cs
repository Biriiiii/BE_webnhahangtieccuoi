using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BE_webnhahangtieccuoi.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleDriveFolderIdToGalleryAlbum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleDriveFolderId",
                table: "GalleryAlbums",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleDriveFolderId",
                table: "GalleryAlbums");
        }
    }
}
