using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DocKick.Data.Migrations
{
    public partial class AddBlobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BlobContainers",
                columns: table => new
                {
                    BlobContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlobContainers", x => x.BlobContainerId);
                });

            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    BlobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlobContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blobs", x => x.BlobId);
                    table.ForeignKey(
                        name: "FK_Blobs_BlobContainers_BlobContainerId",
                        column: x => x.BlobContainerId,
                        principalTable: "BlobContainers",
                        principalColumn: "BlobContainerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blobs_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId",
                unique: true,
                filter: "[ParentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId_Name_ParentId",
                table: "Categories",
                columns: new[] { "UserId", "Name", "ParentId" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [ParentId] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_BlobContainerId",
                table: "Blobs",
                column: "BlobContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_CategoryId",
                table: "Blobs",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blobs");

            migrationBuilder.DropTable(
                name: "BlobContainers");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId_Name_ParentId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");
        }
    }
}
