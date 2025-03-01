namespace RailwayReservation.ViewModels
{
    public class RegisterViewModel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; } // ✅ Ensure it's a simple string, not a navigation property
    }
}
