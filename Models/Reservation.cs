namespace RailwayReservation.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string PRN { get; set; }  // Unique Ticket Number

        [Required, ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }  // Customer Info

        [Required, ForeignKey("Train")]
        public int TrainNo { get; set; }
        public Train Train { get; set; }

        [Required, ForeignKey("Station")]
        public int FromStationId { get; set; }
        public Station FromStation { get; set; }

        [Required, ForeignKey("Station")]
        public int ToStationId { get; set; }
        public Station ToStation { get; set; }

        [Required]
        public DateTime JourneyDate { get; set; }

        [Required]
        public string SeatNo { get; set; }

        [Required]
        public string CoachNo { get; set; }

        [Required]
        public decimal Fare { get; set; }

        public bool IsCancelled { get; set; } = false;
    }
}

