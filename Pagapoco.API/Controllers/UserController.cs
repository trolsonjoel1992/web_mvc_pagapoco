using Microsoft.AspNetCore.Mvc;
using Pagapoco.Services.Interfaces;
using Pagapoco.Core.Entities;
using Pagapoco.API.Dtos;

namespace Pagapoco.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Registro de usuario
        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] UserRegisterDto dto)
        {
            var user = _userService.Register(dto.Email, dto.Password, dto.Name, dto.Phone, dto.City);
            return Ok(user);
        }

        // Login de usuario
        [HttpPost("login")]
        public ActionResult<User?> Login([FromBody] UserLoginDto dto)
        {
            var user = _userService.Login(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized();
            return Ok(user);
        }

        // Actualizar usuario
        [HttpPut("{userId}")]
        public IActionResult UpdateUser(Guid userId, [FromBody] UserUpdateDto dto)
        {
            _userService.UpdateUser(userId, dto.Name, dto.Phone, dto.City);
            return NoContent();
        }

        // Eliminar usuario
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(Guid userId, [FromQuery] bool softDelete = true)
        {
            _userService.DeleteUser(userId, softDelete);
            return NoContent();
        }

        // Publicaciones de un usuario
        [HttpGet("{userId}/publications")]
        public ActionResult<List<Publication>> GetUserPublications(Guid userId)
        {
            var publications = _userService.GetUserPublications(userId);
            return Ok(publications);
        }
    }

    
}
