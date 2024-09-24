using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangoCard.Migrations
{
    /// <inheritdoc />
    public partial class pointsAddedToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_WalletCards_WalletCardWalletId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_WalletCardWalletId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "WalletCardWalletId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Customers");

            migrationBuilder.AddColumn<Guid>(
                name: "WalletCardWalletId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_WalletCardWalletId",
                table: "Customers",
                column: "WalletCardWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_WalletCards_WalletCardWalletId",
                table: "Customers",
                column: "WalletCardWalletId",
                principalTable: "WalletCards",
                principalColumn: "WalletId");
        }
    }
}
