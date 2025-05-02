using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using webapi.Model;
using webapi.ModelDto;

namespace webapi.Endpoints
{
    public static class BookGenreEndPoints
    {
        public static void RegisterBookGenreEndPoints(this WebApplication app)
        {
            var genres = app.MapGroup("/genres")
                .WithTags("Book Genre");

            genres.MapGet("", GetGenres);


            static async Task<Results<Ok<List<Genre>>, BadRequest<string>>> GetGenres(MkarpovCourseworkLibraryContext db)
            {
                return TypedResults.BadRequest("not impemented");
            }
        }
    }
}
