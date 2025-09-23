using Bookings.Domain.Bookings.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:bookings.booking_status", "paid,pending,cancelled");

            migrationBuilder.AddColumn<BookingStatus>(
                name: "status",
                schema: "bookings",
                table: "bookings",
                type: "booking_status",
                nullable: false,
                defaultValue: "paid",
                comment: "Booking status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                schema: "bookings",
                table: "bookings");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:bookings.booking_status", "paid,pending,cancelled");
        }
    }
}
