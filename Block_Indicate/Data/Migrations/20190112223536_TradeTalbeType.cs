using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class TradeTalbeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Closed",
                table: "Trades");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Trades",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Trades");

            migrationBuilder.AddColumn<bool>(
                name: "Closed",
                table: "Trades",
                nullable: false,
                defaultValue: false);
        }
    }
}
