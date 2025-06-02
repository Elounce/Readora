using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webapi.DTOs;
using webapi.Models;

namespace webapi.Endpoints
{
    public static class AuthEndPoints
    {
        public static void RegisterAuthEndPoints(this WebApplication app)
        {
            var auth = app.MapGroup("/auth")
                .WithTags("Authentication");

            auth.MapPost("/login", Login);
            auth.MapPost("/register", Register);


            static async Task<Results<Ok<string>, BadRequest<string>>> Register(
                RegisterRequestDto request,
                IPasswordHasher<User> passwordHasher,
                LibraryContext db)
            {
                if (db.Users.Any(u => u.Email == request.Email))
                {
                    return TypedResults.BadRequest("User with this email already exists.");
                }

                var user = new User
                {
                    Email = request.Email,
                    Nickname = request.Email
                };

                user.HashPassword = passwordHasher.HashPassword(user, request.Password);

                db.Users.Add(user);
                await db.SaveChangesAsync();

                return TypedResults.Ok("User registered successfully.");
            }

            static async Task<Results<Ok<string>, BadRequest<string>>> Login(
                LoginRequestDto request,
                IPasswordHasher<User> passwordHasher,
                IConfiguration config,
                JwtService jwtService,
                LibraryContext db)
            {
                var user = await db.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    return TypedResults.BadRequest("Invalid email or password.");
                }

                var result = passwordHasher.VerifyHashedPassword(user, user.HashPassword, request.Password);
                if (result != PasswordVerificationResult.Success)
                {
                    return TypedResults.BadRequest("Invalid email or password.");
                }

                var role = user.Role?.Name ?? "User";

                string token = jwtService.GenerateToken(user.Email, role);

                return TypedResults.Ok($"{role}: {token}");
            }
        }
    }
}
