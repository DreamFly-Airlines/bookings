using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFareConditionsConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "fare_conditions",
                schema: "bookings",
                table: "ticket_flights",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                comment: "Travel class",
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 10,
                oldComment: "Travel class");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "fare_conditions",
                schema: "bookings",
                table: "ticket_flights",
                type: "integer",
                maxLength: 10,
                nullable: false,
                comment: "Travel class",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldComment: "Travel class");
        }
    }
}
