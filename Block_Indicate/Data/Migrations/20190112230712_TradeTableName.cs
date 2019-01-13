using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class TradeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Trades",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Trades");
        }
    }
}
