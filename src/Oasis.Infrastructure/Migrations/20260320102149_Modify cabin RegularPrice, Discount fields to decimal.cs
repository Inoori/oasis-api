using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oasis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifycabinRegularPriceDiscountfieldstodecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "regularPrice",
                table: "cabins",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "cabins",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "regularPrice",
                table: "cabins",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "discount",
                table: "cabins",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }
    }
}
