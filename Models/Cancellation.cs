namespace RailwayReservation.Models
{

    using System.ComponentModel.DataAnnotations;

    public class Cancellation
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string PRN { get; set; }  // Link to Reservation

        [Required]
        public DateTime CancellationDate { get; set; }

        [Required]
        public decimal RefundAmount { get; set; }
    }
}

