using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace House.Migrations
{
    public partial class LinkCustomerCustomUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                schema: "house",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomUser",
                schema: "house",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserID",
                schema: "house",
                table: "Customer",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomUser_UserID",
                schema: "house",
                table: "Customer",
                column: "UserID",
                principalSchema: "house",
                principalTable: "CustomUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomUser_UserID",
                schema: "house",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "CustomUser",
                schema: "house");

            migrationBuilder.DropIndex(
                name: "IX_Customer_UserID",
                schema: "house",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "UserID",
                schema: "house",
                table: "Customer");
        }
    }
}
