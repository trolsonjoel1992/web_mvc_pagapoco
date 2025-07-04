using Microsoft.AspNetCore.Mvc;
using Pagapoco.Services.Interfaces;
using Pagapoco.API.Dtos;
using Pagapoco.Core.Entities;

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

        // Obtener imágenes de una publicación
        [HttpGet("publication/{publicationId}")]
        public ActionResult<List<Image>> GetPublicationImages(Guid publicationId)
        {
            var images = _imageService.GetPublicationImages(publicationId);
            return Ok(images);
        }

        // Agregar imágenes a una publicación
        [HttpPost("publication/{publicationId}/add")]
        public IActionResult AddImagesToPublication(Guid publicationId, [FromQuery] Guid userId, [FromBody] List<string> imageUrls)
        {
            _imageService.AddImagesToPublication(publicationId, userId, imageUrls);
            return NoContent();
        }

        // Eliminar una imagen
        [HttpDelete("{imageId}")]
        public IActionResult DeleteImage(Guid imageId, [FromQuery] Guid userId)
        {
            _imageService.DeleteImage(imageId, userId);
            return NoContent();
        }

        // Actualizar una imagen
        [HttpPut("{imageId}")]
        public IActionResult UpdateImage(Guid imageId, [FromQuery] Guid userId, [FromBody] ImageUpdateDto dto)
        {
            _imageService.UpdateImage(imageId, userId, dto.Url, dto.AltText, dto.DisplayOrder);
            return NoContent();
        }
    }
}