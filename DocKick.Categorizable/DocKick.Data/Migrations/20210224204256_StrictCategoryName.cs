using Microsoft.EntityFrameworkCore.Migrations;

namespace DocKick.Data.Migrations
{
    public partial class StrictCategoryName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId_Name_ParentId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId_Name_ParentId",
                table: "Categories",
                columns: new[] { "UserId", "Name", "ParentId" },
                unique: true,
                filter: "[ParentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId_Name_ParentId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId_Name_ParentId",
                table: "Categories",
                columns: new[] { "UserId", "Name", "ParentId" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [ParentId] IS NOT NULL");
        }
    }
}
