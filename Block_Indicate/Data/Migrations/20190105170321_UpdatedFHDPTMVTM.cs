using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class UpdatedFHDPTMVTM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BitcoinVolumeToMatch",
                table: "FourHourDojis",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceToMatch",
                table: "FourHourDojis",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TriggeredDojiFourHours",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(nullable: true),
                    Open = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    Close = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    BitcoinVolume = table.Column<decimal>(type: "decimal(28, 18)", nullable: false),
                    RealTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggeredDojiFourHours", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TriggeredDojiFourHours");

            migrationBuilder.DropColumn(
                name: "BitcoinVolumeToMatch",
                table: "FourHourDojis");

            migrationBuilder.DropColumn(
                name: "PriceToMatch",
                table: "FourHourDojis");
        }
    }
}
