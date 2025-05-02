using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webapi.Model;
using webapi.ModelDto;

namespace webapi.Endpoints;

public static class UserEndPoints
{
    public static void RegisterUserEndPoints(this WebApplication app)
    {
        var users = app.MapGroup("/users")
            .WithTags("User");

        users.MapGet("", GetUsers);
        users.MapGet("/user", GetUser);
        users.MapGet("/{id}", GetUserById)
            .RequireAuthorization();
        users.MapPost("", CreateUser);
        users.MapDelete("", DeleteUser)
            .RequireAuthorization();
        users.MapPut("/user", UpdateUser);

        static async Task<Results<Ok<List<UserDtoDetailed>>, NotFound, BadRequest<string>>> GetUsers(MkarpovCourseworkLibraryContext db, [FromQuery] string page = "1", [FromQuery] string limit = "25")
        {
            // Валидация параметров
            if (!int.TryParse(page, out int pageNumber) || !int.TryParse(limit, out int limitNumber))
                return TypedResults.BadRequest("Значения page и limit должны быть числами.");

            if (pageNumber <= 0 || limitNumber <= 0)
                return TypedResults.BadRequest("Значения page и limit должны быть больше нуля.");

            if (limitNumber >= 50)
                return TypedResults.BadRequest("Превышен лимит");

            int skip = (pageNumber - 1) * limitNumber;

            List<User> users = await db.Users
                .Skip(skip)
                .Take(limitNumber)
                .ToListAsync();

            if (!users.Any())
                return TypedResults.NotFound();

            // Преобразование модели Book в DTO (Data Transfer Object) BookDto
            List<UserDtoDetailed> usersDto = users.Select(u => new UserDtoDetailed
            {
                Id = u.Id,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            }).ToList();

            return TypedResults.Ok(usersDto);
        }

        static async Task<Results<Ok<UserDtoDetailed>, NotFound, BadRequest<string>>> GetUser(MkarpovCourseworkLibraryContext db, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return TypedResults.BadRequest("Variables login and password is null");

            UserDtoDetailed? user = await db.Users
                .Select(user => new UserDtoDetailed
                {
                    Id = user.Id,
                    Login = user.Login,
                    HashPassword = user.HashPassword,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Nickname = user.Nickname,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    BirthDate = user.BirthDate,
                    LastLoginDate = user.LastLoginDate,
                    AdultContentRestriction = user.AdultContentRestriction,
                    PublicAccount = user.PublicAccount,
                    RoleId = user.RoleId
                })
                .FirstOrDefaultAsync(u => u.Email == email & u.HashPassword == password);

            if (user == null)
                return TypedResults.NotFound();

            return TypedResults.Ok(user);
        }

        static async Task<Results<Ok, NotFound<string>, UnauthorizedHttpResult>> GetUserById(MkarpovCourseworkLibraryContext db, HttpContext httpContext, int id)
        {
            if (!httpContext.User.Identity?.IsAuthenticated ?? false)
                return TypedResults.Unauthorized();

            User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return TypedResults.NotFound("user is null");

            return TypedResults.Ok();
        }

        static async Task<Results<Created, BadRequest<string>>> CreateUser(MkarpovCourseworkLibraryContext db, UserDto userDto)
        {
            if (userDto is null)
                return TypedResults.BadRequest("User is null");
            if (string.IsNullOrWhiteSpace(userDto.HashPassword) || string.IsNullOrWhiteSpace(userDto.Email))
                return TypedResults.BadRequest("HashPassword or Email is null");
            // Проверка на уникальность email
            if (await db.Users.AnyAsync(u => u.Email == userDto.Email))
                return TypedResults.BadRequest("Email already exists");
            // Установка Nickname при регистрации
            if (string.IsNullOrWhiteSpace(userDto.Nickname))
                userDto.Nickname = userDto.Email.Split('@')[0];

            User user = new User()
            {
                HashPassword = userDto.HashPassword,
                Nickname = userDto.Nickname,
                Email = userDto.Email,
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return TypedResults.Created();
        }

        static async Task<Results<Ok, NotFound, BadRequest<string>>> DeleteUser(
            MkarpovCourseworkLibraryContext db)
        {
            return TypedResults.BadRequest("Превышен лимит");
        }

        static async Task<Results<Ok, NotFound, BadRequest<string>>> UpdateUser(MkarpovCourseworkLibraryContext db)
        {
            return TypedResults.BadRequest("not implemented");
        }
    }
}