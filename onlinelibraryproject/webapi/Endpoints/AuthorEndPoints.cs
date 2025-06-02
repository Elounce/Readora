using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Endpoints
{
    public static class AuthorEndPoints
    {
        public static void RegisterAuthorEndPoints(this WebApplication app)
        {
            var authors = app.MapGroup("/authors")
                .WithTags("Author");

            authors.MapGet("", GetAuthors);


            static async Task<Results<Ok<List<Author>>, BadRequest<string>>> GetAuthors(LibraryContext db)
            {
                var authorsList = await db.Authors.ToListAsync();

                if (authorsList != null && authorsList.Any())
                {
                    return TypedResults.Ok(authorsList);
                }
                else
                {
                    return TypedResults.BadRequest("No authors found in the database.");
                }
            }
        }
    }
}
