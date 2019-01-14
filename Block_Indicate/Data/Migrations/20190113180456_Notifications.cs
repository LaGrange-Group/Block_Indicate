using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class Notifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TelegramDoubleVolume = table.Column<bool>(nullable: false),
                    TelegramFourHourDojis = table.Column<bool>(nullable: false),
                    TelegramSmartTradeConclusions = table.Column<bool>(nullable: false),
                    TelegramBotTradeConclusions = table.Column<bool>(nullable: false),
                    TelegramBotTradeBuys = table.Column<bool>(nullable: false),
                    TelegramSiteUpdates = table.Column<bool>(nullable: false),
                    EmailDoubleVolume = table.Column<bool>(nullable: false),
                    EmailFourHourDojis = table.Column<bool>(nullable: false),
                    EmailSmartTradeConclusions = table.Column<bool>(nullable: false),
                    EmailBotTradeConclusions = table.Column<bool>(nullable: false),
                    EmailBotTradeBuys = table.Column<bool>(nullable: false),
                    EmailSiteUpdates = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CustomerId",
                table: "Notifications",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
