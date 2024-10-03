using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class init10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImgPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.CreateTable(
                name: "Biddings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ToPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDone = table.Column<bool>(type: "bit", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biddings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Biddings_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Auctioneers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    BiddingId = table.Column<int>(type: "int", nullable: false),
                    PercentOfComplete = table.Column<double>(type: "float", nullable: false),
                    IsChosen = table.Column<bool>(type: "bit", nullable: true),
                    StoreId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctioneers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auctioneers_Biddings_BiddingId",
                        column: x => x.BiddingId,
                        principalTable: "Biddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auctioneers_Stores_StoreId1",
                        column: x => x.StoreId1,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BiddingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BiddingId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BiddingCategories_Biddings_BiddingId",
                        column: x => x.BiddingId,
                        principalTable: "Biddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BiddingCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BiddingMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BiddingId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BiddingMaterials_Biddings_BiddingId",
                        column: x => x.BiddingId,
                        principalTable: "Biddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BiddingMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctioneerImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuctioneerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctioneerImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctioneerImages_Auctioneers_AuctioneerId",
                        column: x => x.AuctioneerId,
                        principalTable: "Auctioneers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_BiddingId",
                table: "Materials",
                column: "BiddingId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BiddingId",
                table: "Categories",
                column: "BiddingId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctioneerImages_AuctioneerId",
                table: "AuctioneerImages",
                column: "AuctioneerId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctioneers_BiddingId",
                table: "Auctioneers",
                column: "BiddingId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctioneers_StoreId1",
                table: "Auctioneers",
                column: "StoreId1");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingCategories_BiddingId",
                table: "BiddingCategories",
                column: "BiddingId");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingCategories_CategoryId",
                table: "BiddingCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingMaterials_BiddingId",
                table: "BiddingMaterials",
                column: "BiddingId");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingMaterials_MaterialId",
                table: "BiddingMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Biddings_CustomerId",
                table: "Biddings",
                column: "CustomerId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Biddings_BiddingId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Biddings_BiddingId",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "AuctioneerImages");

            migrationBuilder.DropTable(
                name: "BiddingCategories");

            migrationBuilder.DropTable(
                name: "BiddingMaterials");

            migrationBuilder.DropTable(
                name: "Auctioneers");

            migrationBuilder.DropTable(
                name: "Biddings");

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

            migrationBuilder.AlterColumn<string>(
                name: "ImgPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
