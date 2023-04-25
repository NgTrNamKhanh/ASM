using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_IdentityUserId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_IdentityUserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Order",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdentityUserId",
                table: "Order",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_IdentityUserId",
                table: "Order",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
