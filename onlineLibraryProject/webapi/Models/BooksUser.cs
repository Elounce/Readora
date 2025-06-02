using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class BooksUser
{
    public int UserId { get; set; }

    public int BookId { get; set; }

    public int StatusId { get; set; }

    public decimal? Progress { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual UserReadingStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
