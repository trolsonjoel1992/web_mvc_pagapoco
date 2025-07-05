using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagapoco.API.Dtos;
using Pagapoco.Core.Entities;
using Pagapoco.Services.Interfaces;
using System.Security.Claims;

namespace Pagapoco.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PublicationsController : ControllerBase
    {
        private readonly IPublicationService _publicationService;

        public PublicationsController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        // Paginado público
        [AllowAnonymous]
        [HttpGet("paged")]
        public ActionResult GetPublicationsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (publications, total) = _publicationService.GetPublicationsPaginated(page, pageSize);
            return Ok(new { publications, total });
        }

        // Obtener todas las publicaciones (público)
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<Publication>> GetAll()
        {
            var publications = _publicationService.GetAllPublications();
            return Ok(publications);
        }

        // Buscar publicaciones por ciudad y tipo (público)
        [AllowAnonymous]
        [HttpGet("search")]
        public ActionResult<List<Publication>> Search([FromQuery] string? city, [FromQuery] string? type)
        {
            var result = _publicationService.SearchPublications(city, type);
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
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                City = dto.City,
                IsPremium = dto.IsPremium,
                Type = dto.Type,
                Brand = dto.Brand,
                Model = dto.Model,
                Color = dto.Color,
                Condition = dto.Condition,
                Compatibility = dto.Compatibility,
                UserId = userId
            };
            var created = _publicationService.CreatePublication(publication, userId);
            return Ok(created);
        }

        // Obtener publicación por ID (público)
        [AllowAnonymous]
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

        // Filtrar publicaciones (público)
        [AllowAnonymous]
        [HttpGet("filter")]
        public ActionResult<List<Publication>> Filter(
            [FromQuery] string type,
            [FromQuery] string? brand,
            [FromQuery] string? model,
            [FromQuery] string? color,
            [FromQuery] string? condition,
            [FromQuery] string? compatibility
        )
        {
            var filters = new Dictionary<string, object>();
            if (brand != null) filters["Brand"] = brand;
            if (model != null) filters["Model"] = model;
            if (color != null) filters["Color"] = color;
            if (condition != null) filters["Condition"] = condition;
            if (compatibility != null) filters["Compatibility"] = compatibility;

            var result = _publicationService.FilterPublications(type, filters);
            return Ok(result);
        }

        // Pausar publicación (protegido)
        [HttpPost("{publicationId}/pause")]
        public IActionResult Pause(Guid publicationId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            // Opcional: validar ownership aquí si lo deseas
            _publicationService.PausePublication(publicationId);
            return NoContent();
        }

        // Activar publicación (protegido)
        [HttpPost("{publicationId}/activate")]
        public IActionResult Activate(Guid publicationId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            // Opcional: validar ownership aquí si lo deseas
            _publicationService.ActivatePublication(publicationId);
            return NoContent();
        }
    }
}