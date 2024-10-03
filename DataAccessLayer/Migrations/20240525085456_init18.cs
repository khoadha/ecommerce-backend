using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class init18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Biddings_BiddingId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Biddings_BiddingId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_BiddingId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Categories_BiddingId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "BiddingId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "BiddingId",
                table: "Categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BiddingId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BiddingId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_BiddingId",
                table: "Materials",
                column: "BiddingId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BiddingId",
                table: "Categories",
                column: "BiddingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Biddings_BiddingId",
                table: "Categories",
                column: "BiddingId",
                principalTable: "Biddings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Biddings_BiddingId",
                table: "Materials",
                column: "BiddingId",
                principalTable: "Biddings",
                principalColumn: "Id");
        }
    }
}
