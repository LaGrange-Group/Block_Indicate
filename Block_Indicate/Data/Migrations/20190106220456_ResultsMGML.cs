using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class ResultsMGML : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MinorGain",
                table: "Results",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MinorLoss",
                table: "Results",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinorGain",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "MinorLoss",
                table: "Results");
        }
    }
}
