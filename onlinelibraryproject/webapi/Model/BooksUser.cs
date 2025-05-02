using System;
using System.Collections.Generic;

namespace webapi.Model;

public partial class BooksUser
{
    public int UserId { get; set; }

    public int BookId { get; set; }

    public int StatusId { get; set; }

    public int LastReadPage { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual UserReadingStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
