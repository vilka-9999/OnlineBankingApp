using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBankingApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedBankData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 1,
                columns: new[] { "BankName", "BankType" },
                values: new object[] { "Bank of America", "National" });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "BankId", "BankCountry", "BankName", "BankType" },
                values: new object[] { 2, "USA", "Chase Bank", "National" });

            migrationBuilder.UpdateData(
                table: "Transfers",
                keyColumn: "TransferId",
                keyValue: 1,
                column: "TransferDate",
                value: new DateTime(2024, 11, 24, 21, 33, 42, 621, DateTimeKind.Utc).AddTicks(6610));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 1,
                columns: new[] { "BankName", "BankType" },
                values: new object[] { "BoA", "testBT" });

            migrationBuilder.UpdateData(
                table: "Transfers",
                keyColumn: "TransferId",
                keyValue: 1,
                column: "TransferDate",
                value: new DateTime(2024, 11, 22, 6, 12, 54, 191, DateTimeKind.Utc).AddTicks(2396));
        }
    }
}
