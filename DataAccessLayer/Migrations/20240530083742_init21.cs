using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class init21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerCarouselImages",
                table: "GlobalSettings");

            migrationBuilder.DropColumn(
                name: "BannerImages",
                table: "GlobalSettings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerCarouselImages",
                table: "GlobalSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerImages",
                table: "GlobalSettings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
