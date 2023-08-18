namespace Presentation.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
    }
}
