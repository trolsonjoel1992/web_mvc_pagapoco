using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagapoco.API.Dtos;
using Pagapoco.Core.Entities;
using Pagapoco.Service.Interfaces;
using Pagapoco.Services.Interfaces;
using System.Security.Claims;

namespace Pagapoco.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Por defecto, todos los métodos requieren JWT
    public class PublicationsController : ControllerBase
    {
        private readonly IPublicationService _publicationService;

        public PublicationsController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        // MÉTODO PÚBLICO: paginado
        [AllowAnonymous]
        [HttpGet("paged")]
        public ActionResult GetPublicationsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (publications, total) = _publicationService.GetPublicationsPaginated(page, pageSize);
            return Ok(new { publications, total });
        }

        // MÉTODO PÚBLICO: obtener todas las publicaciones de todos los usuarios
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<Publication>> GetAll()
        {
            // Si tienes un método específico, úsalo. Si no, puedes usar paginado con un pageSize grande.
            var (publications, _) = _publicationService.GetPublicationsPaginated(1, int.MaxValue);
            return Ok(publications);
        }

        // Buscar publicaciones por ciudad y tipo (protegido)
        [AllowAnonymous] 
        [HttpGet("search")]
        public ActionResult<List<Publication>> Search([FromQuery] string? city, [FromQuery] string? publicationType)
        {
            var result = _publicationService.SearchPublications(city, publicationType);
            return Ok(result);
        }

        // Crear publicación (protegido)
        [HttpPost]
        public ActionResult<Publication> Create([FromBody] PublicationCreateDto dto)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var publication = new Publication
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                City = dto.City,
                IsPremium = dto.IsPremium,
                Type = dto.Type,
                UserId = userId
                // Agrega otros campos si es necesario
            };
            var created = _publicationService.CreatePublication(publication, userId);
            return Ok(created);
        }

        // Obtener publicación por ID (protegido)
        
        [HttpGet("{id}")]
        public ActionResult<Publication?> GetById(Guid id, [FromQuery] bool includeImages = true)
        {
            var publication = _publicationService.GetPublicationById(id, includeImages);
            if (publication == null)
                return NotFound();
            return Ok(publication);
        }

        
        // Actualizar publicación (protegido)
        [HttpPut("{publicationId}")]
        public IActionResult Update(Guid publicationId, [FromBody] PublicationUpdateDto dto)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            _publicationService.UpdatePublication(publicationId, userId, dto.Title, dto.Description, dto.Price);
            return NoContent();
        }

        // Eliminar publicación (protegido)
        [HttpDelete("{publicationId}")]
        public IActionResult Delete(Guid publicationId, [FromQuery] bool softDelete = true)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            _publicationService.DeletePublication(publicationId, userId, softDelete);
            return NoContent();
        }

        // Publicaciones de un usuario (protegido)
        [HttpGet("user/{userId}")]
        public ActionResult<List<Publication>> GetUserPublications(Guid userId)
        {
            var publications = _publicationService.GetUserPublications(userId);
            return Ok(publications);
        }

        // Filtrar publicaciones (protegido)
        [AllowAnonymous]
        [HttpGet("filter")]
        public ActionResult<List<Publication>> Filter(
            [FromQuery] string type,
            [FromQuery] string? brand,
            [FromQuery] string? model,
            [FromQuery] int? year,
            [FromQuery] string? color,
            [FromQuery] string? fuelType,
            [FromQuery] string? transmission,
            [FromQuery] string? engineDisplacement,
            [FromQuery] int? kilometersDriven,
            [FromQuery] string? version,
            [FromQuery] int? doors,
            [FromQuery] int? enginePower,
            [FromQuery] string? wheelSize,
            [FromQuery] string? condition,
            [FromQuery] string? compatibility
        )
        {
            var filters = new Dictionary<string, object>();
            if (brand != null) filters["Brand"] = brand;
            if (model != null) filters["Model"] = model;
            if (year != null) filters["Year"] = year;
            if (color != null) filters["Color"] = color;
            if (fuelType != null) filters["FuelType"] = fuelType;
            if (transmission != null) filters["Transmission"] = transmission;
            if (engineDisplacement != null) filters["EngineDisplacement"] = engineDisplacement;
            if (kilometersDriven != null) filters["KilometersDriven"] = kilometersDriven;
            if (version != null) filters["Version"] = version;
            if (doors != null) filters["Doors"] = doors;
            if (enginePower != null) filters["EnginePower"] = enginePower;
            if (wheelSize != null) filters["WheelSize"] = wheelSize;
            if (condition != null) filters["Condition"] = condition;
            if (compatibility != null) filters["Compatibility"] = compatibility;

            var result = _publicationService.FilterPublications(type, filters);
            return Ok(result);
        }
    }
}