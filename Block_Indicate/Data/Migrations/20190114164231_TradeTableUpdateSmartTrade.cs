using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class TradeTableUpdateSmartTrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStopLoss",
                table: "Trades",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTakeProfit",
                table: "Trades",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrailingStopLoss",
                table: "Trades",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrailingTakeProfit",
                table: "Trades",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TrailingStopLossPercent",
                table: "Trades",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TrailingTakeProfitPercent",
                table: "Trades",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStopLoss",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "IsTakeProfit",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "IsTrailingStopLoss",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "IsTrailingTakeProfit",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TrailingStopLossPercent",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TrailingTakeProfitPercent",
                table: "Trades");
        }
    }
}
