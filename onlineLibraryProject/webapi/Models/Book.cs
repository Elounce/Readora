using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int? Pages { get; set; }

    public string? Description { get; set; }

    public int? YearPublished { get; set; }

    public string? CoverImageUrl { get; set; }

    public int GenreId { get; set; }

    public int? LanguageId { get; set; }

    public int PublisherId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal? RatingAverage { get; set; }

    public short? TotalReviews { get; set; }

    public bool? AdultOnly { get; set; }

    public virtual ICollection<BookChangeHistory> BookChangeHistories { get; set; } = new List<BookChangeHistory>();

    public virtual ICollection<BookFormat> BookFormats { get; set; } = new List<BookFormat>();

    public virtual ICollection<BooksUser> BooksUsers { get; set; } = new List<BooksUser>();

    public virtual Genre Genre { get; set; } = null!;

    public virtual Language? Language { get; set; }

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
