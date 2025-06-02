using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class UserReadingStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BooksUser> BooksUsers { get; set; } = new List<BooksUser>();
}
