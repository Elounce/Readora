using Microsoft.AspNetCore.Http.HttpResults;
using webapi.Model;
using webapi.ModelDto;

namespace webapi.Endpoints
{
    public static class PublisherEndPoints
    {
        public static void RegisterPublisherEndPoints(this WebApplication app)
        {
            var publishers = app.MapGroup("/publishers")
                .WithTags("Publisher");

            publishers.MapGet("", GetPublishers);

            static async Task<Results<Ok<List<PublisherDto>>, BadRequest<string>>> GetPublishers(MkarpovCourseworkLibraryContext db)
            {
                return TypedResults.BadRequest("not impemented");
            }
        }
    }
}
