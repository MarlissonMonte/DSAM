using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using app.api.Data;
using app.api.DTOs;
using app.api.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace app.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("doctors")]
        public async Task<ActionResult<List<UserDto>>> GetDoctors()
        {
            var doctors = await _context.Users
                .Where(u => u.UserType == UserType.Doctor)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    UserType = u.UserType.ToString()
                })
                .ToListAsync();

            return Ok(doctors);
        }
    }
} 