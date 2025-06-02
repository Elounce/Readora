using Microsoft.AspNetCore.Http.HttpResults;
using webapi.Models;
using webapi.ModelDto;

namespace webapi.Endpoints
{
    public static class BookCategoryEndPoints
    {
        public static void RegisterBookCategoryEndPoints(this WebApplication app)
        {
            var categories = app.MapGroup("/categories")
                .WithTags("Book Category");

            categories.MapGet("", GetCategories);


            static async Task<Results<Ok<List<CategoryDto>>, BadRequest<string>>> GetCategories(LibraryContext db)
            {
                return TypedResults.BadRequest("not impemented");
            }
        }
    }
}
