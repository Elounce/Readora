using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class BookChangeHistory
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string ActionType { get; set; } = null!;

    public string OldValue { get; set; } = null!;

    public string NewValue { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual Book Book { get; set; } = null!;
}
