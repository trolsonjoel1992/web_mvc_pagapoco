using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagapoco.Services.Interfaces;
using Pagapoco.API.Dtos;
using Pagapoco.Core.Entities;
using System.Security.Claims;

namespace Pagapoco.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        // Obtener imágenes de una publicación (público)
        [AllowAnonymous]
        [HttpGet("publication/{publicationId}")]
        public ActionResult<List<Image>> GetPublicationImages(Guid publicationId)
        {
            var images = _imageService.GetPublicationImages(publicationId);
            return Ok(images);
        }

        // Agregar imágenes a una publicación (privado)
        [Authorize]
        [HttpPost("publication/{publicationId}/add")]
        public IActionResult AddImagesToPublication(Guid publicationId, [FromBody] List<string> imageUrls)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            _imageService.AddImagesToPublication(publicationId, userId, imageUrls);
            return NoContent();
        }

        // Eliminar una imagen (privado)
        [Authorize]
        [HttpDelete("{imageId}")]
        public IActionResult DeleteImage(Guid imageId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            _imageService.DeleteImage(imageId, userId);
            return NoContent();
        }

        // Actualizar una imagen (privado)
        [Authorize]
        [HttpPut("{imageId}")]
        public IActionResult UpdateImage(Guid imageId, [FromBody] ImageUpdateDto dto)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            _imageService.UpdateImage(imageId, userId, dto.Url, dto.AltText, dto.DisplayOrder);
            return NoContent();
        }
    }
}