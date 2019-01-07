using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class TradeBotTableTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeBots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    AllMarkets = table.Column<bool>(nullable: false),
                    AllocatedBitcoin = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    PercentTakeProfit = table.Column<decimal>(type: "decimal(4, 4)", nullable: false),
                    PercentStopLoss = table.Column<decimal>(type: "decimal(4, 4)", nullable: false),
                    Exchange = table.Column<string>(nullable: true),
                    BaseCurrency = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeBots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeBots_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeBots_CustomerId",
                table: "TradeBots",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeBots");
        }
    }
}
