using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using app.api.Data;
using app.api.DTOs;
using app.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace app.api.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return null;

            var token = GenerateJwtToken(user);
            var userDto = MapToUserDto(user);

            return new AuthResponse
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            // Verificar se o email já existe
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return null;

            // Validar tipo de usuário
            if (!Enum.TryParse<UserType>(request.UserType, out var userType))
                return null;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword,
                Phone = request.Phone,
                UserType = userType
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var userDto = MapToUserDto(user);

            return new AuthResponse
            {
                Token = token,
                User = userDto
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "your-secret-key-here"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserType", user.UserType.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                UserType = user.UserType.ToString()
            };
        }
    }
} 