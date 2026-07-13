using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialMonitor.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Transactions",
                newName: "Timestamp");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Transactions",
                newName: "CreatedAt");
        }
    }
}
