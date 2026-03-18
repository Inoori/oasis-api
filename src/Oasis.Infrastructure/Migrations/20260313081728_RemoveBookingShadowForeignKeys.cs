using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oasis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookingShadowForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_cabins_cabinID1",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_guests_guestID1",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_cabinID1",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_guestID1",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "cabinID1",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "guestID1",
                table: "bookings");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_cabinID",
                table: "bookings",
                column: "cabinID");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_guestID",
                table: "bookings",
                column: "guestID");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_cabins_cabinID",
                table: "bookings",
                column: "cabinID",
                principalTable: "cabins",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_guests_guestID",
                table: "bookings",
                column: "guestID",
                principalTable: "guests",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_cabins_cabinID",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_guests_guestID",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_cabinID",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_guestID",
                table: "bookings");

            migrationBuilder.AddColumn<long>(
                name: "cabinID1",
                table: "bookings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "guestID1",
                table: "bookings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_cabinID1",
                table: "bookings",
                column: "cabinID1");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_guestID1",
                table: "bookings",
                column: "guestID1");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_cabins_cabinID1",
                table: "bookings",
                column: "cabinID1",
                principalTable: "cabins",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_guests_guestID1",
                table: "bookings",
                column: "guestID1",
                principalTable: "guests",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
