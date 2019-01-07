using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class UpdateResultDS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseAF",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "HighAF",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "LowAF",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "MaxPercentGain24Hr",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "MaxPercentLoss24Hr",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "MinorGain",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "OpenAF",
                table: "Results",
                newName: "MaxPercentDiff24Hr");

            migrationBuilder.RenameColumn(
                name: "MinorLoss",
                table: "Results",
                newName: "NoTrade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NoTrade",
                table: "Results",
                newName: "MinorLoss");

            migrationBuilder.RenameColumn(
                name: "MaxPercentDiff24Hr",
                table: "Results",
                newName: "OpenAF");

            migrationBuilder.AddColumn<decimal>(
                name: "CloseAF",
                table: "Results",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HighAF",
                table: "Results",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LowAF",
                table: "Results",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxPercentGain24Hr",
                table: "Results",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxPercentLoss24Hr",
                table: "Results",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "MinorGain",
                table: "Results",
                nullable: true);
        }
    }
}
