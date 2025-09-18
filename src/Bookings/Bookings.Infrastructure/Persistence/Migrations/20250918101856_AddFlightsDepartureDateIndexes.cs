using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightsDepartureDateIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "flights_actual_departure_with_status_cond_idx",
                schema: "bookings",
                table: "flights",
                column: "actual_departure",
                filter: "status = 'Delayed'");

            migrationBuilder.CreateIndex(
                name: "flights_scheduled_departure_with_status_cond_idx",
                schema: "bookings",
                table: "flights",
                column: "scheduled_departure",
                filter: "status in ('Scheduled', 'On time')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "flights_actual_departure_with_status_cond_idx",
                schema: "bookings",
                table: "flights");

            migrationBuilder.DropIndex(
                name: "flights_scheduled_departure_with_status_cond_idx",
                schema: "bookings",
                table: "flights");
        }
    }
}
