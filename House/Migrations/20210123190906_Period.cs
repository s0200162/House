using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace House.Migrations
{
    public partial class Period : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginTime",
                schema: "house",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "house",
                table: "Reservation");

            migrationBuilder.AddColumn<int>(
                name: "PeriodID",
                schema: "house",
                table: "Reservation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Place",
                schema: "house",
                table: "Location",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Period",
                schema: "house",
                columns: table => new
                {
                    PeriodID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hour = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Period", x => x.PeriodID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_PeriodID",
                schema: "house",
                table: "Reservation",
                column: "PeriodID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Period_PeriodID",
                schema: "house",
                table: "Reservation",
                column: "PeriodID",
                principalSchema: "house",
                principalTable: "Period",
                principalColumn: "PeriodID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Period_PeriodID",
                schema: "house",
                table: "Reservation");

            migrationBuilder.DropTable(
                name: "Period",
                schema: "house");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_PeriodID",
                schema: "house",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "PeriodID",
                schema: "house",
                table: "Reservation");

            migrationBuilder.AddColumn<DateTime>(
                name: "BeginTime",
                schema: "house",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                schema: "house",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Place",
                schema: "house",
                table: "Location",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
