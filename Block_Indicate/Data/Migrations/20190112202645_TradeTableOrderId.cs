using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class TradeTableOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "Trades",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Trades");
        }
    }
}
