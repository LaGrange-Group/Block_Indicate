using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class UpdateCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BittrexApiSecret",
                table: "Customers",
                newName: "HuobiApiSecret");

            migrationBuilder.RenameColumn(
                name: "BittrexApiKey",
                table: "Customers",
                newName: "HuobiApiKey");

            migrationBuilder.AddColumn<bool>(
                name: "ConnectedBinance",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConnectedHuobi",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectedBinance",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ConnectedHuobi",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "HuobiApiSecret",
                table: "Customers",
                newName: "BittrexApiSecret");

            migrationBuilder.RenameColumn(
                name: "HuobiApiKey",
                table: "Customers",
                newName: "BittrexApiKey");
        }
    }
}
