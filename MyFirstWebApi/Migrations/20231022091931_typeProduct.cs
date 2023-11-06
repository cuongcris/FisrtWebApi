using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFirstWebApi.Migrations
{
    /// <inheritdoc />
    public partial class typeProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductType",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType", x => x.TypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_TypeId",
                table: "Product",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductType_TypeId",
                table: "Product",
                column: "TypeId",
                principalTable: "ProductType",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductType_TypeId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "ProductType");

            migrationBuilder.DropIndex(
                name: "IX_Product_TypeId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Product");
        }
    }
}
