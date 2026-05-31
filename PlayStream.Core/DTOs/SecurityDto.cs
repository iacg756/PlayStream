using PlayStream.Core.Enum;

namespace PlayStream.Core.DTOs
{
    public class SecurityDto
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Role { get; set; }  
    }
}