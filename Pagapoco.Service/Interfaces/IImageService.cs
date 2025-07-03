namespace Pagapoco.Services.Interfaces;

using Pagapoco.Core.Entities;

public interface IImageService
{
    /// Agrega nuevas imágenes a una publicación existente (requiere ownership)
    void AddImagesToPublication(Guid publicationId, Guid userId, List<string> imageUrls);

    /// Elimina una imagen existente por su ID (valida ownership)
    void DeleteImage(Guid imageId, Guid userId);

    /// Actualiza los datos de una imagen existente (ej: URL, texto alternativo u orden)
    void UpdateImage(
        Guid imageId,
        Guid userId,
        string? newUrl = null,
        string? newAltText = null,
        int? newOrder = null
    );

    /// Obtiene todas las imágenes asociadas a una publicación (sin validar ownership)
    List<Image> GetPublicationImages(Guid publicationId);
}

//public interface IImageService
//{
//    Task<IEnumerable<Image>> GetByPublicationIdAsync(Guid publicationId);
//    Task AddImageAsync(Guid publicationId, Image image);
//    Task RemoveAsync(Guid imageId);
//}
