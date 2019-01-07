using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class BinanceDoubleVolume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoubleVolumeBinance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(nullable: true),
                    LastPrice = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    BitcoinVolume = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    PercentageChange = table.Column<double>(nullable: false),
                    RealTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubleVolumeBinance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValidDoubleVolumeBinance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(nullable: true),
                    Open = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    Close = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    Vwap = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    PercentageChange = table.Column<double>(nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    BitcoinVolume = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    RealTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidDoubleVolumeBinance", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoubleVolumeBinance");

            migrationBuilder.DropTable(
                name: "ValidDoubleVolumeBinance");
        }
    }
}
