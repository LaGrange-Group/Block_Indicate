using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class ResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(nullable: true),
                    OpenAF = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    CloseAF = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    HighAF = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    LowAF = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    Vwap = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    PercentageChangeAF = table.Column<double>(nullable: false),
                    VolumeAF = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    BitcoinVolumeAF = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    BitcoinVolumeOriginal = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    PrevRowId = table.Column<int>(nullable: false),
                    ResultOfTrade = table.Column<bool>(nullable: true),
                    Open = table.Column<bool>(nullable: false),
                    TimeToResult = table.Column<string>(nullable: true),
                    MaxPercentGain24Hr = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    MaxPercentLoss24Hr = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Exchange = table.Column<string>(nullable: true),
                    RealTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");
        }
    }
}
