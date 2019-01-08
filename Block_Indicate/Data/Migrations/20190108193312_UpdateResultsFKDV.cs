using Microsoft.EntityFrameworkCore.Migrations;

namespace Block_Indicate.Data.Migrations
{
    public partial class UpdateResultsFKDV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Results_PrevRowId",
                table: "Results",
                column: "PrevRowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_DoubleVolumeBinance_PrevRowId",
                table: "Results",
                column: "PrevRowId",
                principalTable: "DoubleVolumeBinance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_DoubleVolumeBinance_PrevRowId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_PrevRowId",
                table: "Results");
        }
    }
}
