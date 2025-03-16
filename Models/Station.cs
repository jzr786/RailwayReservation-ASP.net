namespace RailwayReservation.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Station
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string Code { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Division { get; set; }
    }
}