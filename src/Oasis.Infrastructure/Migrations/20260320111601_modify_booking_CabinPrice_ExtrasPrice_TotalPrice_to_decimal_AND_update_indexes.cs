using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oasis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modify_booking_CabinPrice_ExtrasPrice_TotalPrice_to_decimal_AND_update_indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "totalPrice",
                table: "bookings",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "extrasPrice",
                table: "bookings",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "cabinPrice",
                table: "bookings",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_status_startdate",
                table: "bookings",
                columns: new[] { "status", "startDate" });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_status_totalprice",
                table: "bookings",
                columns: new[] { "status", "totalPrice" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_bookings_status_startdate",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_status_totalprice",
                table: "bookings");

            migrationBuilder.AlterColumn<float>(
                name: "totalPrice",
                table: "bookings",
                type: "real",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "extrasPrice",
                table: "bookings",
                type: "real",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "cabinPrice",
                table: "bookings",
                type: "real",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }
    }
}
