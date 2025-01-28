using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlteredTransactionColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerShare",
                table: "Transactions",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ExecutedAt",
                table: "Transactions",
                newName: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Transactions",
                newName: "ExecutedAt");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Transactions",
                newName: "PricePerShare");
        }
    }
}
