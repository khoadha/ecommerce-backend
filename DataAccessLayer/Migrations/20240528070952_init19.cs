using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class init19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctioneers_Stores_StoreId1",
                table: "Auctioneers");

            migrationBuilder.DropIndex(
                name: "IX_Auctioneers_StoreId1",
                table: "Auctioneers");

            migrationBuilder.DropColumn(
                name: "StoreId1",
                table: "Auctioneers");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "Auctioneers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Auctioneers_StoreId",
                table: "Auctioneers",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctioneers_Stores_StoreId",
                table: "Auctioneers",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctioneers_Stores_StoreId",
                table: "Auctioneers");

            migrationBuilder.DropIndex(
                name: "IX_Auctioneers_StoreId",
                table: "Auctioneers");

            migrationBuilder.AlterColumn<string>(
                name: "StoreId",
                table: "Auctioneers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StoreId1",
                table: "Auctioneers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auctioneers_StoreId1",
                table: "Auctioneers",
                column: "StoreId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctioneers_Stores_StoreId1",
                table: "Auctioneers",
                column: "StoreId1",
                principalTable: "Stores",
                principalColumn: "Id");
        }
    }
}
