using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class UpdateTradePerformanceNoTradeAVG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DVBNoTrade",
                table: "TradePerformances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DVBNoTradeAvgSettle",
                table: "TradePerformances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FourDBNoTrade",
                table: "TradePerformances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FourDBNoTradeAvgSettle",
                table: "TradePerformances",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DVBNoTrade",
                table: "TradePerformances");

            migrationBuilder.DropColumn(
                name: "DVBNoTradeAvgSettle",
                table: "TradePerformances");

            migrationBuilder.DropColumn(
                name: "FourDBNoTrade",
                table: "TradePerformances");

            migrationBuilder.DropColumn(
                name: "FourDBNoTradeAvgSettle",
                table: "TradePerformances");
        }
    }
}
