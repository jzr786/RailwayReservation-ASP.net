namespace RailwayReservation.Models
{

    using System.ComponentModel.DataAnnotations;

    public class Train
    {
        [Key]
        public int TrainNo { get; set; }  // Unique Train Number (Primary Key)

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public bool IsUp { get; set; }  // True = Upward direction, False = Downward

        [Required]
        public int TotalCoaches { get; set; }

        [Required]
        public int Ac1Seats { get; set; }

        [Required]
        public int Ac3Seats { get; set; }

        [Required]
        public int SleeperSeats { get; set; }
    }
}

