using Microsoft.EntityFrameworkCore.Migrations;

namespace House.Migrations
{
    public partial class InvoiceTotalPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                schema: "house",
                table: "Invoice",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "house",
                table: "Invoice");
        }
    }
}
