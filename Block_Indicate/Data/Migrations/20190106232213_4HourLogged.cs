using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class _4HourLogged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Logged",
                table: "TriggeredDojiFourHours",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Logged",
                table: "FourHourDojis",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logged",
                table: "TriggeredDojiFourHours");

            migrationBuilder.DropColumn(
                name: "Logged",
                table: "FourHourDojis");
        }
    }
}
