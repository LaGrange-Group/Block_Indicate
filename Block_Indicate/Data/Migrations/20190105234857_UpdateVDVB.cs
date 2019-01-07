using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class UpdateVDVB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Close",
                table: "ValidDoubleVolumeBinance");

            migrationBuilder.DropColumn(
                name: "High",
                table: "ValidDoubleVolumeBinance");

            migrationBuilder.DropColumn(
                name: "Low",
                table: "ValidDoubleVolumeBinance");

            migrationBuilder.DropColumn(
                name: "Open",
                table: "ValidDoubleVolumeBinance");

            migrationBuilder.AddColumn<int>(
                name: "PrevRowId",
                table: "ValidDoubleVolumeBinance",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrevRowId",
                table: "ValidDoubleVolumeBinance");

            migrationBuilder.AddColumn<decimal>(
                name: "Close",
                table: "ValidDoubleVolumeBinance",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "High",
                table: "ValidDoubleVolumeBinance",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Low",
                table: "ValidDoubleVolumeBinance",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Open",
                table: "ValidDoubleVolumeBinance",
                type: "decimal(28, 18)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
