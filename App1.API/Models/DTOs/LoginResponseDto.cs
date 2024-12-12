namespace App1.API.Models.DTOs
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public List<string> roles { get; set; }

        public string token { get; set; }
    }
}
