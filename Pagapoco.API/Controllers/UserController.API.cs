using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pagapoco.Services.Interfaces;
using Pagapoco.Core.Entities;
using Pagapoco.API.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pagapoco.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        // Registro de usuario (público)
        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] UserRegisterDto dto)
        {
            var user = _userService.Register(dto.Email, dto.Password, null, null, null);
            return Ok(user);
        }

        // Login de usuario (público, devuelve JWT)
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginDto dto)
        {
            var user = _userService.Login(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized();

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim("userId", user.Id.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Modificación: incluir el userId en la respuesta
            return Ok(new { token = tokenString, userId = user.Id });
        }

        // Actualizar usuario (requiere JWT)
        [Authorize]
        [HttpPut("{userId}")]
        public IActionResult UpdateUser(Guid userId, [FromBody] UserUpdateDto dto)
        {
            // Opcional: validar que el userId del token coincide con el userId del parámetro
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || userIdClaim != userId.ToString())
                return Forbid();

            _userService.UpdateUser(userId, dto.Name, dto.Phone, dto.City);
            return NoContent();
        }

        // Eliminar usuario (requiere JWT)
        [Authorize]
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(Guid userId, [FromQuery] bool softDelete = true)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || userIdClaim != userId.ToString())
                return Forbid();

            _userService.DeleteUser(userId, softDelete);
            return NoContent();
        }

        // Publicaciones de un usuario (requiere JWT)
        [Authorize]
        [HttpGet("{userId}/publications")]
        public ActionResult<List<Publication>> GetUserPublications(Guid userId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || userIdClaim != userId.ToString())
                return Forbid();

            var publications = _userService.GetUserPublications(userId);
            return Ok(publications);
        }

        // Obtener datos del usuario (requiere JWT)
        [Authorize]
        [HttpGet("{userId}")]
        public ActionResult<User> GetUser(Guid userId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || userIdClaim != userId.ToString())
                return Forbid();

            var user = _userService.GetById(userId);
            if (user == null)
                return NotFound();

            // Opcional: puedes mapear a un DTO para no exponer PasswordHash/Salt
            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Phone,
                user.City
            });
        }
    }
}