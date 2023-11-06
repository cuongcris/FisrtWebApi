using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFirstWebApi.Migrations
{
    /// <inheritdoc />
    public partial class addUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discount",
                table: "Product",
                newName: "Discount");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 3, 13, 50, 25, 231, DateTimeKind.Local).AddTicks(754),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 22, 22, 24, 34, 503, DateTimeKind.Local).AddTicks(6711));

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Product",
                newName: "discount");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 22, 22, 24, 34, 503, DateTimeKind.Local).AddTicks(6711),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 3, 13, 50, 25, 231, DateTimeKind.Local).AddTicks(754));
        }
    }
}
