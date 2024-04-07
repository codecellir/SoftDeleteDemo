using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftDeleteDemo.Migrations
{
    /// <inheritdoc />
    public partial class addFilteredIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Books_IsDeleted",
                table: "Books",
                column: "IsDeleted",
                filter: "IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_IsDeleted",
                table: "Books");
        }
    }
}
