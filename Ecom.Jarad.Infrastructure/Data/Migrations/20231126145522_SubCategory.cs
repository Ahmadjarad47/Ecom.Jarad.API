using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Jarad.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SubCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_SubCategory_CategoryId",
                table: "category");

            migrationBuilder.DropTable(
                name: "SubCategory");

            migrationBuilder.DropIndex(
                name: "IX_category_CategoryId",
                table: "category");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "category");
        }
    }
}
