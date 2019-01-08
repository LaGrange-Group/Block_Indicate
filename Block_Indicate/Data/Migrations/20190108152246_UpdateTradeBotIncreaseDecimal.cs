using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class UpdateTradeBotIncreaseDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PercentTakeProfit",
                table: "TradeBots",
                type: "decimal(28, 18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentStopLoss",
                table: "TradeBots",
                type: "decimal(28, 18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4, 4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PercentTakeProfit",
                table: "TradeBots",
                type: "decimal(4, 4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentStopLoss",
                table: "TradeBots",
                type: "decimal(4, 4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 18)");
        }
    }
}
