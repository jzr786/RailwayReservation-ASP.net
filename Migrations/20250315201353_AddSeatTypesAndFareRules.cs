using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservation.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatTypesAndFareRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fare",
                table: "TrainSchedules",
                newName: "SleeperFarePerKm");

            migrationBuilder.RenameColumn(
                name: "AvailableSeats",
                table: "TrainSchedules",
                newName: "SleeperSeats");

            migrationBuilder.AddColumn<decimal>(
                name: "AC1FarePerKm",
                table: "TrainSchedules",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AC1Seats",
                table: "TrainSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AC3FarePerKm",
                table: "TrainSchedules",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AC3Seats",
                table: "TrainSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Distance",
                table: "TrainSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SeatType",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AC1FarePerKm",
                table: "TrainSchedules");

            migrationBuilder.DropColumn(
                name: "AC1Seats",
                table: "TrainSchedules");

            migrationBuilder.DropColumn(
                name: "AC3FarePerKm",
                table: "TrainSchedules");

            migrationBuilder.DropColumn(
                name: "AC3Seats",
                table: "TrainSchedules");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "TrainSchedules");

            migrationBuilder.DropColumn(
                name: "SeatType",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "SleeperSeats",
                table: "TrainSchedules",
                newName: "AvailableSeats");

            migrationBuilder.RenameColumn(
                name: "SleeperFarePerKm",
                table: "TrainSchedules",
                newName: "Fare");
        }
    }
}
