using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace House.Migrations
{
    public partial class InitialCreateHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "house");

            migrationBuilder.CreateTable(
                name: "Location",
                schema: "house",
                columns: table => new
                {
                    LocationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Place = table.Column<string>(nullable: true),
                    Adress = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationID);
                });

            migrationBuilder.CreateTable(
                name: "Profession",
                schema: "house",
                columns: table => new
                {
                    ProfessionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profession", x => x.ProfessionID);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                schema: "house",
                columns: table => new
                {
                    RoomID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    PriceHour = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_Room_Location_LocationID",
                        column: x => x.LocationID,
                        principalSchema: "house",
                        principalTable: "Location",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "house",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(nullable: false),
                    Lastname = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    ProfessionID = table.Column<int>(nullable: false),
                    Admin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_Customer_Profession_ProfessionID",
                        column: x => x.ProfessionID,
                        principalSchema: "house",
                        principalTable: "Profession",
                        principalColumn: "ProfessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                schema: "house",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Paid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_Invoice_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalSchema: "house",
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                schema: "house",
                columns: table => new
                {
                    ReservationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(nullable: false),
                    RoomID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    BeginTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_Reservation_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalSchema: "house",
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Room_RoomID",
                        column: x => x.RoomID,
                        principalSchema: "house",
                        principalTable: "Room",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ProfessionID",
                schema: "house",
                table: "Customer",
                column: "ProfessionID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CustomerID",
                schema: "house",
                table: "Invoice",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_CustomerID",
                schema: "house",
                table: "Reservation",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_RoomID",
                schema: "house",
                table: "Reservation",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Room_LocationID",
                schema: "house",
                table: "Room",
                column: "LocationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoice",
                schema: "house");

            migrationBuilder.DropTable(
                name: "Reservation",
                schema: "house");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "house");

            migrationBuilder.DropTable(
                name: "Room",
                schema: "house");

            migrationBuilder.DropTable(
                name: "Profession",
                schema: "house");

            migrationBuilder.DropTable(
                name: "Location",
                schema: "house");
        }
    }
}
