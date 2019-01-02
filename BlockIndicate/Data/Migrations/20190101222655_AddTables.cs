using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockIndicate.Data.Migrations
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BinanceData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(nullable: true),
                    Close = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    CloseTime = table.Column<DateTime>(nullable: true),
                    High = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    Open = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    OpenTime = table.Column<DateTime>(nullable: true),
                    QuoteAssetVolume = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    TakerBuyBaseAssetVolume = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    TakerBuyQuoteAssetVolume = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    TradeCount = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    BitcoinVolume = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    PercentageChange = table.Column<decimal>(type: "decimal(18, 5)", nullable: false),
                    RealTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinanceData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    BinanceApiKey = table.Column<string>(nullable: true),
                    BinanceApiSecret = table.Column<string>(nullable: true),
                    BittrexApiKey = table.Column<string>(nullable: true),
                    BittrexApiSecret = table.Column<string>(nullable: true),
                    AccountActive = table.Column<bool>(nullable: false),
                    AccountTrial = table.Column<bool>(nullable: false),
                    TrialStartDate = table.Column<DateTime>(nullable: false),
                    PaidStartDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.FirstName);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DojiFinders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DojiUpperWick = table.Column<double>(nullable: false),
                    DojiLowerWick = table.Column<double>(nullable: false),
                    DojiUpperBody = table.Column<double>(nullable: false),
                    DojiLowerBody = table.Column<double>(nullable: false),
                    DojiVolumeIncrease = table.Column<double>(nullable: false),
                    DojiPercentageIncrease = table.Column<double>(nullable: false),
                    DojiTextMessage = table.Column<bool>(nullable: false),
                    DojiEmail = table.Column<bool>(nullable: false),
                    AllMarkets = table.Column<bool>(nullable: false),
                    AllExchanges = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DojiFinders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exchanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Markets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinanceData");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "DojiFinders");

            migrationBuilder.DropTable(
                name: "Exchanges");

            migrationBuilder.DropTable(
                name: "Markets");
        }
    }
}
