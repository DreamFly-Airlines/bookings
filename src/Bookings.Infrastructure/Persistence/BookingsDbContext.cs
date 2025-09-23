using Bookings.Application.Bookings.ReadModels;
using Bookings.Application.Bookings.ReadModels.ReadModels;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Entities;
using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;
using Bookings.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Bookings.Infrastructure.Persistence;

public partial class BookingsDbContext : DbContext
{
    public BookingsDbContext()
    {
    }

    public BookingsDbContext(DbContextOptions<BookingsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aircraft> Aircrafts { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<BoardingPass> BoardingPasses { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FlightReadModel> FlightsView { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketFlight> TicketFlights { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // NOTE: ForeignKeyIndexConvention creates indexes that do not exist in the bookings schema
        // for the file "demo-big.zip 2018-01-11 22:28 232M" on https://edu.postgrespro.ru/, these are
        // flights.aircraft_code, flights.arrival_airport, flights.departure_airport,
        // tickets.book_ref, ticket_flights.flight_id
        
        configurationBuilder.Conventions.Remove<ForeignKeyIndexConvention>();
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aircraft>(entity =>
        {
            entity.HasKey(a => a.AircraftCode);
            entity.ToView("aircrafts", "bookings");

            entity.Property(e => e.AircraftCode)
                .HasConversion(new StringBackedDataConverter<AircraftCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Aircraft code, IATA")
                .HasColumnName("aircraft_code");
            entity.Property(e => e.Model)
                .HasComment("Aircraft model")
                .HasColumnName("model");
            entity.Property(e => e.Range)
                .HasComment("Maximal flying distance, km")
                .HasColumnName("range");
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(a => a.AirportCode);
            entity.ToView("airports", "bookings");

            entity.Property(e => e.AirportCode)
                .HasConversion(new StringBackedDataConverter<IataAirportCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Airport code")
                .HasColumnName("airport_code");
            entity.Property(e => e.AirportName)
                .HasComment("Airport name")
                .HasColumnName("airport_name");
            entity.Property(e => e.City)
                .HasComment("City")
                .HasColumnName("city");
            entity.Property(e => e.Coordinates)
                .HasComment("Airport coordinates (longitude and latitude)")
                .HasConversion(new CoordinatesConverter())
                .HasColumnName("coordinates");
            entity.Property(e => e.Timezone)
                .HasConversion(new StringBackedDataConverter<IanaTimezone>())
                .HasComment("Airport time zone")
                .HasColumnName("timezone");
        });

        modelBuilder.Entity<BoardingPass>(entity =>
        {
            entity.HasKey(e => new { e.TicketNo, e.FlightId }).HasName("boarding_passes_pkey");

            entity.ToTable("boarding_passes", "bookings", tb => tb.HasComment("Boarding passes"));

            entity.HasIndex(e => new { e.FlightId, e.BoardingNo }, "boarding_passes_flight_id_boarding_no_key").IsUnique();

            entity.HasIndex(e => new { e.FlightId, e.SeatNo }, "boarding_passes_flight_id_seat_no_key").IsUnique();

            entity.Property(e => e.TicketNo)
                .HasConversion(new StringBackedDataConverter<TicketNo>())
                .HasMaxLength(13)
                .IsFixedLength()
                .HasComment("Ticket number")
                .HasColumnName("ticket_no");
            entity.Property(e => e.FlightId)
                .HasComment("Flight ID")
                .HasColumnName("flight_id");
            entity.Property(e => e.BoardingNo)
                .HasComment("Boarding pass number")
                .HasColumnName("boarding_no");
            entity.Property(e => e.SeatNo)
                .HasMaxLength(4)
                .HasComment("Seat number")
                .HasColumnName("seat_no");
            entity.HasOne<TicketFlight>()
                .WithOne()
                .HasForeignKey<BoardingPass>(d => new { d.TicketNo, d.FlightId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("boarding_passes_ticket_no_fkey");
        });

        const string bookingStatusColumnName = "booking_status";
        modelBuilder.HasPostgresEnum<BookingStatus>(name: bookingStatusColumnName);
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookRef)
                .HasName("bookings_pkey");

            entity.ToTable("bookings", "bookings", tb => tb.HasComment("Bookings"));

            entity.Property(e => e.BookRef)
                .HasConversion(new StringBackedDataConverter<BookRef>())
                .HasMaxLength(6)
                .IsFixedLength()
                .HasComment("Booking number")
                .HasColumnName("book_ref");
            entity.Property(e => e.BookDate)
                .HasComment("Booking date")
                .HasColumnName("book_date");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasComment("Total booking cost")
                .HasColumnName("total_amount");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType(bookingStatusColumnName)
                .HasComment("Booking status");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("flights_pkey");

            entity.ToTable("flights", "bookings", tb => tb.HasComment("Flights"));

            entity.HasIndex(e => new { e.FlightNo, e.ScheduledDeparture }, "flights_flight_no_scheduled_departure_key").IsUnique();

            entity.Property(e => e.FlightId)
                .HasDefaultValueSql("nextval('flights_flight_id_seq'::regclass)")
                .HasComment("Flight ID")
                .HasColumnName("flight_id");
            entity.Property(e => e.ActualArrival)
                .HasComment("Actual arrival time")
                .HasColumnName("actual_arrival");
            entity.Property(e => e.ActualDeparture)
                .HasComment("Actual departure time")
                .HasColumnName("actual_departure");
            entity.Property(e => e.AircraftCode)
                .HasConversion(new StringBackedDataConverter<AircraftCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Aircraft code, IATA")
                .HasColumnName("aircraft_code");
            entity.Property(e => e.ArrivalAirport)
                .HasConversion(new StringBackedDataConverter<IataAirportCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Airport of arrival")
                .HasColumnName("arrival_airport");
            entity.Property(e => e.DepartureAirport)
                .HasConversion(new StringBackedDataConverter<IataAirportCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Airport of departure")
                .HasColumnName("departure_airport");
            entity.Property(e => e.FlightNo)
                .HasConversion(new StringBackedDataConverter<FlightNo>())
                .HasMaxLength(6)
                .IsFixedLength()
                .HasComment("Flight number")
                .HasColumnName("flight_no");
            entity.Property(e => e.ScheduledArrival)
                .HasComment("Scheduled arrival time")
                .HasColumnName("scheduled_arrival");
            entity.Property(e => e.ScheduledDeparture)
                .HasComment("Scheduled departure time")
                .HasColumnName("scheduled_departure");
            entity.Property(e => e.Status)
                .HasConversion(new FlightStatusConverter())
                .HasMaxLength(20)
                .HasComment("Flight status")
                .HasColumnName("status");
            entity.HasOne<Aircraft>()
                .WithMany()
                .HasForeignKey(d => d.AircraftCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("flights_aircraft_code_fkey");
            entity.HasOne<Airport>()
                .WithMany()
                .HasForeignKey(d => d.ArrivalAirport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("flights_arrival_airport_fkey");
            entity.HasOne<Airport>()
                .WithMany()
                .HasForeignKey(d => d.DepartureAirport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("flights_departure_airport_fkey");

            entity.HasIndex(f => f.ScheduledDeparture)
                .HasFilter("status in ('Scheduled', 'On time')")
                .HasDatabaseName("flights_scheduled_departure_with_status_cond_idx");
            entity.HasIndex(f => f.ActualDeparture)
                .HasFilter("status = 'Delayed'")
                .HasDatabaseName("flights_actual_departure_with_status_cond_idx");
        });

        modelBuilder.Entity<FlightReadModel>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("flights_v", "bookings");

            entity.Property(e => e.ActualArrival)
                .HasComment("Actual arrival time")
                .HasColumnName("actual_arrival");
            entity.Property(e => e.ActualArrivalLocal)
                .HasComment("Actual arrival time, local time at the point of destination")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actual_arrival_local");
            entity.Property(e => e.ActualDeparture)
                .HasComment("Actual departure time")
                .HasColumnName("actual_departure");
            entity.Property(e => e.ActualDepartureLocal)
                .HasComment("Actual departure time, local time at the point of departure")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actual_departure_local");
            entity.Property(e => e.ActualDuration)
                .HasComment("Actual flight duration")
                .HasColumnName("actual_duration");
            entity.Property(e => e.AircraftCode)
                .HasConversion(new StringBackedDataConverter<AircraftCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Aircraft code, IATA")
                .HasColumnName("aircraft_code");
            entity.Property(e => e.ArrivalAirport)
                .HasConversion(new StringBackedDataConverter<IataAirportCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Arrival airport code")
                .HasColumnName("arrival_airport");
            entity.Property(e => e.ArrivalAirportName)
                .HasComment("Arrival airport name")
                .HasColumnName("arrival_airport_name");
            entity.Property(e => e.ArrivalCity)
                .HasComment("City of arrival")
                .HasColumnName("arrival_city");
            entity.Property(e => e.DepartureAirport)
                .HasConversion(new StringBackedDataConverter<IataAirportCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Deprature airport code")
                .HasColumnName("departure_airport");
            entity.Property(e => e.DepartureAirportName)
                .HasComment("Departure airport name")
                .HasColumnName("departure_airport_name");
            entity.Property(e => e.DepartureCity)
                .HasComment("City of departure")
                .HasColumnName("departure_city");
            entity.Property(e => e.FlightId)
                .HasComment("Flight ID")
                .HasColumnName("flight_id");
            entity.Property(e => e.FlightNo)
                .HasConversion(new StringBackedDataConverter<FlightNo>())
                .HasMaxLength(6)
                .IsFixedLength()
                .HasComment("Flight number")
                .HasColumnName("flight_no");
            entity.Property(e => e.ScheduledArrival)
                .HasComment("Scheduled arrival time")
                .HasColumnName("scheduled_arrival");
            entity.Property(e => e.ScheduledArrivalLocal)
                .HasComment("Scheduled arrival time, local time at the point of destination")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("scheduled_arrival_local");
            entity.Property(e => e.ScheduledDeparture)
                .HasComment("Scheduled departure time")
                .HasColumnName("scheduled_departure");
            entity.Property(e => e.ScheduledDepartureLocal)
                .HasComment("Scheduled departure time, local time at the point of departure")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("scheduled_departure_local");
            entity.Property(e => e.ScheduledDuration)
                .HasComment("Scheduled flight duration")
                .HasColumnName("scheduled_duration");
            entity.Property(e => e.Status)
                .HasConversion(new FlightStatusConverter())
                .HasMaxLength(20)
                .HasComment("Flight status")
                .HasColumnName("status");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("routes", "bookings");

            entity.Property(e => e.AircraftCode)
                .HasConversion(new StringBackedDataConverter<AircraftCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Aircraft code, IATA")
                .HasColumnName("aircraft_code");
            entity.Property(e => e.ArrivalAirport)
                .HasConversion(new StringBackedDataConverter<IataAirportCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Code of airport of arrival")
                .HasColumnName("arrival_airport");
            entity.Property(e => e.ArrivalAirportName)
                .HasComment("Name of airport of arrival")
                .HasColumnName("arrival_airport_name");
            entity.Property(e => e.ArrivalCity)
                .HasComment("City of arrival")
                .HasColumnName("arrival_city");
            entity.Property(e => e.DaysOfWeek)
                .HasConversion(new WeekDayListConverter())
                .HasComment("Days of week on which flights are scheduled")
                .HasColumnName("days_of_week");
            entity.Property(e => e.DepartureAirport)
                .HasConversion(new StringBackedDataConverter<IataAirportCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Code of airport of departure")
                .HasColumnName("departure_airport");
            entity.Property(e => e.DepartureAirportName)
                .HasComment("Name of airport of departure")
                .HasColumnName("departure_airport_name");
            entity.Property(e => e.DepartureCity)
                .HasComment("City of departure")
                .HasColumnName("departure_city");
            entity.Property(e => e.Duration)
                .HasComment("Scheduled duration of flight")
                .HasColumnName("duration");
            entity.Property(e => e.FlightNo)
                .HasConversion(new StringBackedDataConverter<FlightNo>())
                .HasMaxLength(6)
                .IsFixedLength()
                .HasComment("Flight number")
                .HasColumnName("flight_no");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => new { e.AircraftCode, e.SeatNo }).HasName("seats_pkey");

            entity.ToTable("seats", "bookings", tb => tb.HasComment("Seats"));

            entity.Property(e => e.AircraftCode)
                .HasConversion(new StringBackedDataConverter<AircraftCode>())
                .HasMaxLength(3)
                .IsFixedLength()
                .HasComment("Aircraft code, IATA")
                .HasColumnName("aircraft_code");
            entity.Property(e => e.SeatNo)
                .HasMaxLength(4)
                .HasComment("Seat number")
                .HasColumnName("seat_no");
            entity.Property(e => e.FareConditions)
                .HasConversion(new FareConditionsConverter())
                .HasMaxLength(10)
                .HasComment("Travel class")
                .HasColumnName("fare_conditions");
            entity.HasOne<Aircraft>()
                .WithMany()
                .HasForeignKey(d => d.AircraftCode)
                .HasConstraintName("seats_aircraft_code_fkey");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketNo).HasName("tickets_pkey");

            entity.ToTable("tickets", "bookings", tb => tb.HasComment("Tickets"));

            entity.Property(e => e.TicketNo)
                .HasConversion(new StringBackedDataConverter<TicketNo>())
                .HasMaxLength(13)
                .IsFixedLength()
                .HasComment("Ticket number")
                .HasColumnName("ticket_no");
            entity.Property(e => e.BookRef)
                .HasConversion(new StringBackedDataConverter<BookRef>())
                .HasMaxLength(6)
                .IsFixedLength()
                .HasComment("Booking number")
                .HasColumnName("book_ref");
            entity.Property(e => e.ContactData)
                .HasConversion(new ContactDataConverter())
                .HasComment("Passenger contact information")
                .HasColumnType("jsonb")
                .HasColumnName("contact_data");
            entity.Property(e => e.PassengerId)
                .HasMaxLength(20)
                .HasComment("Passenger ID")
                .HasColumnName("passenger_id");
            entity.Property(e => e.PassengerName)
                .HasComment("Passenger name")
                .HasColumnName("passenger_name");
            entity.HasOne<Booking>()
                .WithMany(b => b.Tickets)
                .HasForeignKey(d => d.BookRef)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tickets_book_ref_fkey");
        });

        modelBuilder.Entity<TicketFlight>(entity =>
        {
            entity.HasKey(e => new { e.TicketNo, e.FlightId }).HasName("ticket_flights_pkey");

            entity.ToTable("ticket_flights", "bookings", tb => tb.HasComment("Flight segment"));

            entity.Property(e => e.TicketNo)
                .HasConversion(new StringBackedDataConverter<TicketNo>())
                .HasMaxLength(13)
                .IsFixedLength()
                .HasComment("Ticket number")
                .HasColumnName("ticket_no");
            entity.Property(e => e.FlightId)
                .HasComment("Flight ID")
                .HasColumnName("flight_id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasComment("Travel cost")
                .HasColumnName("amount");
            entity.Property(e => e.FareConditions)
                .HasConversion(new  FareConditionsConverter())
                .HasMaxLength(10)
                .HasComment("Travel class")
                .HasColumnName("fare_conditions");
            entity.HasOne<Flight>()
                .WithMany()
                .HasForeignKey(d => d.FlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ticket_flights_flight_id_fkey");
            entity.HasOne<Ticket>()
                .WithMany(t => t.TicketFlights)
                .HasForeignKey(d => d.TicketNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ticket_flights_ticket_no_fkey");
            
            entity.HasIndex(tf => tf.FlightId)
                .HasDatabaseName("ticket_flights_flight_id_idx");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
