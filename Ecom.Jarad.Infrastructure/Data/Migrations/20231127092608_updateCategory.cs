using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Jarad.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_SubCategory_CategoryId",
                table: "category");

            migrationBuilder.DropIndex(
                name: "IX_category_CategoryId",
                table: "category");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "category");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_CategoryId",
                table: "SubCategory",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategory_category_CategoryId",
                table: "SubCategory",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategory_category_CategoryId",
                table: "SubCategory");

            migrationBuilder.DropIndex(
                name: "IX_SubCategory_CategoryId",
                table: "SubCategory");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_CategoryId",
                table: "category",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_category_SubCategory_CategoryId",
                table: "category",
                column: "CategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id");
        }
    }
}
