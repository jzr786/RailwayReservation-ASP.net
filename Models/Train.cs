namespace RailwayReservation.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Train
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure Identity column
        public int TrainNo { get; set; }  // Auto-generated ID

        public string Name { get; set; }
        public bool IsUp { get; set; }
        public int TotalCoaches { get; set; }
        public int Ac1Seats { get; set; }
        public int Ac3Seats { get; set; }
        public int SleeperSeats { get; set; }
    }

}