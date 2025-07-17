using Microsoft.AspNetCore.Mvc;
using app.api.DTOs;
using app.api.Services;

namespace app.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.LoginAsync(request);
            
            if (response == null)
                return Unauthorized(new { message = "Email ou senha inválidos" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RegisterAsync(request);
            
            if (response == null)
                return BadRequest(new { message = "Email já cadastrado ou dados inválidos" });

            return CreatedAtAction(nameof(Register), response);
        }
    }
} 