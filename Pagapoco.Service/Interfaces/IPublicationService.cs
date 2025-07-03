namespace Pagapoco.Service.Interfaces;

using Pagapoco.Core.Entities;
using System.Collections.Generic;

/// Contrato para el servicio de gestión de publicaciones (Vehicle, Bike, Part)
public interface IPublicationService
{
    /// Obtiene publicaciones paginadas con el total de registros
    (List<Publication> Publications, int TotalCount) GetPublicationsPaginated(int page, int pageSize);

    /// Busca publicaciones por ciudad y tipo (Vehicle/Bike/Part)
    List<Publication> SearchPublications(string? city, string? publicationType);

    /// Crea una nueva publicación (requiere usuario logueado)
    Publication CreatePublication(Publication publication, Guid userId);

    /// Obtiene una publicación por ID
    Publication? GetPublicationById(Guid id, bool includeImages = true);

    /// Actualiza una publicación existente (solo para el usuario creador)
    void UpdatePublication(Guid publicationId, Guid userId, string title, string description, decimal price);

    /// Elimina una publicación (física o lógicamente)
    void DeletePublication(Guid publicationId, Guid userId, bool softDelete = true);

    /// Obtiene las publicaciones de un usuario específico
    List<Publication> GetUserPublications(Guid userId);
}
//public interface IPublicationService
 //{
 //    /// Obtiene una publicación por su ID.
 //    Task<Publication?> GetByIdAsync(Guid id);

//    /// Filtra publicaciones por ciudad y/o tipo de publicación.
//    Task<IEnumerable<Publication>> SearchAsync(string? city, string? category);

//    /// Crea una nueva publicación.
//    Task CreateAsync(Publication publication);

//    /// Edita una publicación existente.
//    Task UpdateAsync(Publication publication);

//    /// Elimina permanentemente una publicación.
//    Task DeleteAsync(Guid publicationId);

//    /// Pausa una publicación sin eliminarla.
//    Task PauseAsync(Guid publicationId);

//    /// Pausa o reactiva una publicación.
//    Task SetPauseStateAsync(Guid publicationId, bool isPaused);
//}