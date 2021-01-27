using Microsoft.EntityFrameworkCore.Migrations;

namespace House.Migrations
{
    public partial class ReservationInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservationInvoice",
                schema: "house",
                columns: table => new
                {
                    ReservationInvoiceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationID = table.Column<int>(nullable: false),
                    InvoiceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationInvoice", x => x.ReservationInvoiceID);
                    table.ForeignKey(
                        name: "FK_ReservationInvoice_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "house",
                        principalTable: "Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ReservationInvoice_Reservation_ReservationID",
                        column: x => x.ReservationID,
                        principalSchema: "house",
                        principalTable: "Reservation",
                        principalColumn: "ReservationID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationInvoice_InvoiceID",
                schema: "house",
                table: "ReservationInvoice",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationInvoice_ReservationID",
                schema: "house",
                table: "ReservationInvoice",
                column: "ReservationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationInvoice",
                schema: "house");
        }
    }
}
