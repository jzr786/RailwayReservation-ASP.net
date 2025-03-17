using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservation.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TrainScheduleId { get; set; }

        [ForeignKey("TrainScheduleId")]
        public TrainSchedule TrainSchedule { get; set; }

        [Required]
        public string UserId { get; set; } 

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int FromStationId { get; set; }

        [ForeignKey("FromStationId")]
        public Station FromStation { get; set; }

        [Required]
        public int ToStationId { get; set; }

        [ForeignKey("ToStationId")]
        public Station ToStation { get; set; }

        [Required]
        public int NumberOfSeats { get; set; } 

        [Required]
        public DateTime ReservationTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalFare { get; set; }

        [Required]
        public string SeatType { get; set; } 
    }
}