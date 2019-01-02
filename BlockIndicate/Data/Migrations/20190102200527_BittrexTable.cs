using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockIndicate.Data.Migrations
{
    public partial class BittrexTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BittrexData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(nullable: true),
                    High = table.Column<decimal>(type: "decimal(18, 10)", nullable: true),
                    Low = table.Column<decimal>(type: "decimal(18, 10)", nullable: true),
                    Volume = table.Column<decimal>(type: "decimal(18, 10)", nullable: true),
                    Last = table.Column<decimal>(type: "decimal(18, 10)", nullable: true),
                    BaseVolume = table.Column<decimal>(type: "decimal(18, 10)", nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Bid = table.Column<decimal>(type: "decimal(18, 10)", nullable: true),
                    Ask = table.Column<decimal>(type: "decimal(18, 10)", nullable: true),
                    OpenBuyOrders = table.Column<int>(nullable: true),
                    OpenSellOrders = table.Column<int>(nullable: true),
                    PrevDay = table.Column<decimal>(type: "decimal(18, 10)", nullable: true),
                    PercentageChange = table.Column<decimal>(type: "decimal(18, 10)", nullable: true),
                    RealTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BittrexData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BittrexData");
        }
    }
}
