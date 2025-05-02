using System;
using System.Collections.Generic;

namespace webapi.Model;

public partial class User
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string HashPassword { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Nickname { get; set; }

    public string Email { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public bool? AdultContentRestriction { get; set; }

    public bool? PublicAccount { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<BooksUser> BooksUsers { get; set; } = new List<BooksUser>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual UserRole? Role { get; set; }
}
