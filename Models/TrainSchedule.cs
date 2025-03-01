namespace RailwayReservation.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TrainSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Train")]
        public int TrainNo { get; set; }
        public Train Train { get; set; }  // Navigation Property

        [Required, ForeignKey("Station")]
        public int StationId { get; set; }
        public Station Station { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public int DistanceFromStart { get; set; } // Distance from starting station
    }
}
