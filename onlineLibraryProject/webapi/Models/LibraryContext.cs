using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookChangeHistory> BookChangeHistories { get; set; }

    public virtual DbSet<BookFormat> BookFormats { get; set; }

    public virtual DbSet<BooksUser> BooksUsers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Format> Formats { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Recommendation> Recommendations { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserReadingStatus> UserReadingStatuses { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Author");

            entity.HasIndex(e => e.Name, "author_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Book");

            entity.HasIndex(e => e.GenreId, "Book_Genre_FK");

            entity.HasIndex(e => new { e.Title, e.Description }, "FT_Book_title_desc");

            entity.HasIndex(e => e.Isbn, "UX_Book_isbn").IsUnique();

            entity.HasIndex(e => e.LanguageId, "book_language_FK");

            entity.HasIndex(e => e.PublisherId, "book_publisher_FK");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdultOnly)
                .HasDefaultValueSql("'0'")
                .HasColumnName("adultOnly");
            entity.Property(e => e.CoverImageUrl)
                .HasColumnType("text")
                .HasColumnName("coverImageUrl");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.GenreId).HasColumnName("genreId");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("isbn");
            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.Pages).HasColumnName("pages");
            entity.Property(e => e.Price)
                .HasPrecision(10)
                .HasColumnName("price");
            entity.Property(e => e.PublisherId).HasColumnName("publisherId");
            entity.Property(e => e.RatingAverage)
                .HasPrecision(3)
                .HasColumnName("ratingAverage");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TotalReviews).HasColumnName("totalReviews");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.YearPublished)
                .HasColumnType("year")
                .HasColumnName("yearPublished");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Book_Genre_FK");

            entity.HasOne(d => d.Language).WithMany(p => p.Books)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("book_language_FK");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("book_publisher_FK");

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("BA_author_fk"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("BA_book_fk"),
                    j =>
                    {
                        j.HasKey("BookId", "AuthorId").HasName("PRIMARY");
                        j.ToTable("BookAuthor");
                        j.HasIndex(new[] { "AuthorId" }, "BA_author_fk");
                        j.IndexerProperty<int>("BookId").HasColumnName("bookId");
                        j.IndexerProperty<int>("AuthorId").HasColumnName("authorId");
                    });

            entity.HasMany(d => d.Categories).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("bookCategory_category_FK"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("bookCategory_book_FK"),
                    j =>
                    {
                        j.HasKey("BookId", "CategoryId").HasName("PRIMARY");
                        j.ToTable("BookCategory");
                        j.HasIndex(new[] { "CategoryId" }, "bookCategory_category_FK");
                        j.IndexerProperty<int>("BookId").HasColumnName("bookId");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("categoryId");
                    });
        });

        modelBuilder.Entity<BookChangeHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("BookChangeHistory");

            entity.HasIndex(e => e.BookId, "BookHistory_Book_FK");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(100)
                .HasColumnName("actionType");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.NewValue)
                .HasColumnType("text")
                .HasColumnName("newValue");
            entity.Property(e => e.OldValue)
                .HasColumnType("text")
                .HasColumnName("oldValue");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Book).WithMany(p => p.BookChangeHistories)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BookHistory_Book_FK");
        });

        modelBuilder.Entity<BookFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("BookFormat");

            entity.HasIndex(e => new { e.BookId, e.FormatId }, "bookId").IsUnique();

            entity.HasIndex(e => e.FormatId, "formatId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.FileSize).HasColumnName("fileSize");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(255)
                .HasColumnName("fileUrl");
            entity.Property(e => e.FormatId).HasColumnName("formatId");

            entity.HasOne(d => d.Book).WithMany(p => p.BookFormats)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BookFormat_ibfk_1");

            entity.HasOne(d => d.Format).WithMany(p => p.BookFormats)
                .HasForeignKey(d => d.FormatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BookFormat_ibfk_2");
        });

        modelBuilder.Entity<BooksUser>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.BookId }).HasName("PRIMARY");

            entity.ToTable("BooksUser");

            entity.HasIndex(e => e.StatusId, "BooksUser_UserReadingStatus_FK");

            entity.HasIndex(e => e.BookId, "BooksUser_book_FK");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.FinishDate)
                .HasColumnType("datetime")
                .HasColumnName("finishDate");
            entity.Property(e => e.Progress)
                .HasPrecision(5)
                .HasColumnName("progress");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Book).WithMany(p => p.BooksUsers)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BooksUser_book_FK");

            entity.HasOne(d => d.Status).WithMany(p => p.BooksUsers)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BooksUser_UserReadingStatus_FK");

            entity.HasOne(d => d.User).WithMany(p => p.BooksUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BooksUser_user_FK");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "category_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Format>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Format");

            entity.HasIndex(e => e.Name, "format_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Genre");

            entity.HasIndex(e => e.Name, "Genre_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Language");

            entity.HasIndex(e => e.NativeName, "Language_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Name, "name_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
            entity.Property(e => e.NativeName)
                .HasMaxLength(64)
                .HasColumnName("nativeName");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Publisher");

            entity.HasIndex(e => e.Name, "publisher_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Recommendation");

            entity.HasIndex(e => e.BookId, "Rec_book_fk");

            entity.HasIndex(e => e.UserId, "Rec_user_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasColumnName("reason");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Book).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rec_book_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rec_user_fk");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Review");

            entity.HasIndex(e => e.Rating, "IX_Review_rating");

            entity.HasIndex(e => e.BookId, "Review_Book_FK");

            entity.HasIndex(e => e.UserId, "Review_User_FK");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Comment)
                .HasColumnType("text")
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Rating)
                .HasPrecision(2, 1)
                .HasColumnName("rating");
            entity.Property(e => e.UpdatedAt)
                .HasMaxLength(100)
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Review_Book_FK");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Review_User_FK");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.RoleId, "User_UserRole_FK");

            entity.HasIndex(e => e.Email, "email_unique").IsUnique();

            entity.HasIndex(e => e.Login, "login_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdultContentRestriction).HasColumnName("adultContentRestriction");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birthDate");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("firstName");
            entity.Property(e => e.HashPassword)
                .HasMaxLength(255)
                .HasColumnName("hashPassword");
            entity.Property(e => e.LastLoginDate)
                .HasColumnType("datetime")
                .HasColumnName("lastLoginDate");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("lastName");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Nickname)
                .HasMaxLength(20)
                .HasColumnName("nickname");
            entity.Property(e => e.PublicAccount).HasColumnName("publicAccount");
            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("'1'")
                .HasColumnName("roleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("User_UserRole_FK");
        });

        modelBuilder.Entity<UserReadingStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserReadingStatus");

            entity.HasIndex(e => e.Name, "UserReadingStatus_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserRole");

            entity.HasIndex(e => e.Name, "UserRole_UNIQUE_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
