using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using webapi.Models;
using webapi.ModelDto;

namespace webapi.Endpoints
{
    public static class BookEndPoints
    {
        public static void RegisterBookEndpoints(this WebApplication app)
        {

            var books = app.MapGroup("/books")
                .WithTags("Book");

            books.MapGet("", GetBooks);
            books.MapGet("/book/{id}", GetBookById);
            books.MapGet("/random", GetRandomBooks);


            // GET Books by page
            static async Task<Results<Ok<List<BookDto>>, NotFound, BadRequest<string>>> GetBooks(
                IMemoryCache cache,
                LibraryContext db,
                [FromQuery] string? sortOrder = "title",
                [FromQuery] string page = "1",
                [FromQuery] string limit = "25")
            {
                // Validation logic
                if (!int.TryParse(page, out int pageNumber) || !int.TryParse(limit, out int limitNumber))
                    return TypedResults.BadRequest("The page and limit values must be numbers.");
                if (pageNumber <= 0 || limitNumber <= 0)
                    return TypedResults.BadRequest("The page and limit values must be greater than zero.");
                if (limitNumber >= 50)
                    return TypedResults.BadRequest("Exceeded the limit");

                var books = await db.Books
                    .Skip((pageNumber - 1) * limitNumber)
                    .Take(limitNumber)
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title
                    })
                    .ToListAsync();

                return books.Any() ? TypedResults.Ok(books) : TypedResults.NotFound();
            }


            // GET BookById
            static async Task<Results<Ok<Book>, NotFound>> GetBookById(
                int id,
                LibraryContext db)
            {
                return await db.Books.FindAsync(id)
                    is Book book ? TypedResults.Ok(book) : TypedResults.NotFound();
            }




            static async Task<Results<Ok<List<BookDto>>, NotFound, BadRequest<string>>> GetRandomBooks(
                LibraryContext db,
                [FromQuery] string limit = "5")
            {
                if (!int.TryParse(limit, out int limitNumber) && limitNumber <= 0)
                {
                    return TypedResults.BadRequest("The limit value must be greater than zero.");
                }

                int skip;
                int maxRandomValue = await db.Books.CountAsync() - limitNumber;
                if (maxRandomValue > 0)
                    skip = new Random().Next(0, maxRandomValue);
                else
                    skip = 10;


                var books = await db.Books
                    .Skip(skip)
                    .Take(limitNumber)
                    .Select(book => new BookDto
                    {
                        Id = book.Id,
                        Title = book.Title
                    })
                    .ToListAsync();

                if (!books.Any())
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(books);
            }
        }
    }
}
