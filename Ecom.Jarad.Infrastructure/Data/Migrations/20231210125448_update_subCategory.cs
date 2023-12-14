using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Jarad.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_subCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountItems",
                table: "SubCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountItems",
                table: "SubCategory");
        }
    }
}
