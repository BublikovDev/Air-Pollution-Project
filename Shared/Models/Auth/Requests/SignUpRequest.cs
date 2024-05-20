using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Auth.Requests
{
    public class SignUpRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
