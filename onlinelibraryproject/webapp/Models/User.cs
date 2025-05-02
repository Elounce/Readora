namespace webapp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; } = null!;

        public string HashPassword { get; set; } = null!;

        public string? Name { get; set; }

        public string Email { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public int? RoleId { get; set; }
    }
}
