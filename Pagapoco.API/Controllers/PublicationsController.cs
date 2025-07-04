using Microsoft.AspNetCore.Mvc;
using Pagapoco.API.Dtos;
using Pagapoco.Core.Entities;
using Pagapoco.Service.Interfaces;
using Pagapoco.Services.Interfaces;

namespace Pagapoco.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicationsController : ControllerBase
    {
        private readonly IPublicationService _publicationService;

        public PublicationsController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        // Obtener publicaciones paginadas
        [HttpGet("paged")]
        public ActionResult GetPublicationsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (publications, total) = _publicationService.GetPublicationsPaginated(page, pageSize);
            return Ok(new { publications, total });
        }

        // Buscar publicaciones por ciudad y tipo
        [HttpGet("search")]
        public ActionResult<List<Publication>> Search([FromQuery] string? city, [FromQuery] string? publicationType)
        {
            var result = _publicationService.SearchPublications(city, publicationType);
            return Ok(result);
        }

        // Crear publicación (Vehicle, Bike, Part)
        [HttpPost]
        public ActionResult<Publication> Create([FromBody] PublicationCreateDto dto)
        {
            Publication publication;

            switch (dto.Type)
            {
                case "Vehicle":
                    publication = new Vehicle
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        Price = dto.Price,
                        City = dto.City,
                        IsPremium = dto.IsPremium,
                        Type = dto.Type,
                        UserId = dto.UserId,
                        Brand = dto.Brand ?? "",
                        Model = dto.Model ?? "",
                        Year = dto.Year ?? 0,
                        Color = dto.Color ?? "",
                        FuelType = dto.FuelType ?? "",
                        Transmission = dto.Transmission ?? "",
                        EngineDisplacement = dto.EngineDisplacement ?? "",
                        KilometersDriven = dto.KilometersDriven ?? 0,
                        Version = dto.Version ?? "",
                        Doors = dto.Doors ?? 0
                    };
                    break;
                case "Bike":
                    publication = new Bike
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        Price = dto.Price,
                        City = dto.City,
                        IsPremium = dto.IsPremium,
                        Type = dto.Type,
                        UserId = dto.UserId,
                        Brand = dto.Brand ?? "",
                        Model = dto.Model ?? "",
                        Year = dto.Year ?? 0,
                        Color = dto.Color ?? "",
                        FuelType = dto.FuelType ?? "",
                        Transmission = dto.Transmission ?? "",
                        EngineDisplacement = dto.EngineDisplacement ?? "",
                        KilometersDriven = dto.KilometersDriven ?? 0,
                        EnginePower = dto.EnginePower ?? 0,
                        WheelSize = dto.WheelSize ?? ""
                    };
                    break;
                case "Part":
                    publication = new Part
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        Price = dto.Price,
                        City = dto.City,
                        IsPremium = dto.IsPremium,
                        Type = dto.Type,
                        UserId = dto.UserId,
                        Brand = dto.Brand ?? "",
                        Model = dto.Model ?? "",
                        Color = dto.Color ?? "",
                        Condition = dto.Condition ?? "",
                        Compatibility = dto.Compatibility ?? ""
                    };
                    break;
                default:
                    return BadRequest("Tipo de publicación no válido.");
            }

            var created = _publicationService.CreatePublication(publication, dto.UserId);
            return Ok(created);
        }

        // Obtener publicación por ID
        [HttpGet("{id}")]
        public ActionResult<Publication?> GetById(Guid id, [FromQuery] bool includeImages = true)
        {
            var publication = _publicationService.GetPublicationById(id, includeImages);
            if (publication == null)
                return NotFound();
            return Ok(publication);
        }

        // Actualizar publicación
        [HttpPut("{publicationId}")]
        public IActionResult Update(Guid publicationId, [FromQuery] Guid userId, [FromBody] PublicationUpdateDto dto)
        {
            _publicationService.UpdatePublication(publicationId, userId, dto.Title, dto.Description, dto.Price);
            return NoContent();
        }

        // Eliminar publicación
        [HttpDelete("{publicationId}")]
        public IActionResult Delete(Guid publicationId, [FromQuery] Guid userId, [FromQuery] bool softDelete = true)
        {
            _publicationService.DeletePublication(publicationId, userId, softDelete);
            return NoContent();
        }

        // Publicaciones de un usuario
        [HttpGet("user/{userId}")]
        public ActionResult<List<Publication>> GetUserPublications(Guid userId)
        {
            var publications = _publicationService.GetUserPublications(userId);
            return Ok(publications);
        }

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
         // Construir diccionario de filtros
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