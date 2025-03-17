using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservation.Models
{
    public class TrainSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TrainNo { get; set; }

        [ForeignKey("TrainNo")]
        public Train Train { get; set; }

        [Required]
        public int FromStationId { get; set; }

        [ForeignKey("FromStationId")]
        public Station FromStation { get; set; }

        [Required]
        public int ToStationId { get; set; }

        [ForeignKey("ToStationId")]
        public Station ToStation { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public int Distance { get; set; } 

        
        public int AC1Seats { get; set; }
        public decimal AC1FarePerKm { get; set; }

        public int AC3Seats { get; set; }
        public decimal AC3FarePerKm { get; set; }

        public int SleeperSeats { get; set; }
        public decimal SleeperFarePerKm { get; set; }
    }
}
