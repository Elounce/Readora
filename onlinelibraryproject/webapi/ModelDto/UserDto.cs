namespace webapi.ModelDto
{
    public class UserDto
    {
        public string HashPassword { get; set; } = null!;

        public string? Nickname { get; set; }

        public string Email { get; set; } = null!;
    }
}
