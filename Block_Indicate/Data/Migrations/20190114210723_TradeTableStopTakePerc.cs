using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class TradeTableStopTakePerc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMarket",
                table: "Trades",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "StopLossPercent",
                table: "Trades",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TakeProfitPercent",
                table: "Trades",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMarket",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "StopLossPercent",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TakeProfitPercent",
                table: "Trades");
        }
    }
}
