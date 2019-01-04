using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class Customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    BinanceApiKey = table.Column<string>(nullable: true),
                    BinanceApiSecret = table.Column<string>(nullable: true),
                    BittrexApiKey = table.Column<string>(nullable: true),
                    BittrexApiSecret = table.Column<string>(nullable: true),
                    AccountActive = table.Column<bool>(nullable: false),
                    AccountTrial = table.Column<bool>(nullable: false),
                    CompletedSignUp = table.Column<bool>(nullable: false),
                    TrialStartDate = table.Column<DateTime>(nullable: false),
                    PaidStartDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
