using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class TradeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(nullable: true),
                    Exchange = table.Column<string>(nullable: true),
                    BuyPrice = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    TakeProfit = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    StopLoss = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    StartingBitcoinAmount = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    EndingBitcoinAmount = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    CurrentPercentageResult = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    FinalPercentageResult = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    TimeActive = table.Column<string>(nullable: true),
                    TimeToCompletion = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Closed = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    RealTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trades_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trades_CustomerId",
                table: "Trades",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trades");
        }
    }
}
