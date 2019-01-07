using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class TradePerformance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradePerformances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DVBSuccess = table.Column<double>(nullable: false),
                    DVBFail = table.Column<double>(nullable: false),
                    DVBAvgTime = table.Column<string>(nullable: true),
                    FourDBAvgTime = table.Column<string>(nullable: true),
                    FourDBSuccess = table.Column<double>(nullable: false),
                    FourDBFail = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradePerformances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradePerformances");
        }
    }
}
