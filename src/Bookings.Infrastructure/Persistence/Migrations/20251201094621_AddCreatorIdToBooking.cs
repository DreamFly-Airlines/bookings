using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorIdToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "creator_id",
                schema: "bookings",
                table: "bookings",
                type: "text",
                nullable: true,
                comment: "User that made the booking");

            migrationBuilder.Sql(@"
                UPDATE bookings.bookings
                SET creator_id = gen_random_uuid()::text
                WHERE creator_id IS NULL;");

            migrationBuilder.AlterColumn<string>(
                name: "creator_id",
                schema: "bookings",
                table: "bookings",
                type: "text",
                nullable: false,
                comment: "User that made the booking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creator_id",
                schema: "bookings",
                table: "bookings");
        }
    }
}
