namespace webapi.ModelDto
{
    public class UserDto
    {
        public string? Login { get; set; }

        public string Password { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Nickname { get; set; }

        public string Email { get; set; } = null!;

        public DateTime? BirthDate { get; set; }

        public bool? AdultContentRestriction { get; set; }

        public bool? PublicAccount { get; set; }
    }
}
