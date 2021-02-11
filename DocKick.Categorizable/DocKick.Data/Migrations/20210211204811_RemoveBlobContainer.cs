using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DocKick.Data.Migrations
{
    public partial class RemoveBlobContainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blobs_BlobContainers_BlobContainerId",
                table: "Blobs");

            migrationBuilder.DropTable(
                name: "BlobContainers");

            migrationBuilder.DropIndex(
                name: "IX_Blobs_BlobContainerId",
                table: "Blobs");

            migrationBuilder.DropColumn(
                name: "BlobContainerId",
                table: "Blobs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BlobContainerId",
                table: "Blobs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BlobContainers",
                columns: table => new
                {
                    BlobContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlobContainers", x => x.BlobContainerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_BlobContainerId",
                table: "Blobs",
                column: "BlobContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_BlobContainers_Name",
                table: "BlobContainers",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlobContainers_UserId",
                table: "BlobContainers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Blobs_BlobContainers_BlobContainerId",
                table: "Blobs",
                column: "BlobContainerId",
                principalTable: "BlobContainers",
                principalColumn: "BlobContainerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
