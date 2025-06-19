namespace Pagapoco.Core.Interfaces;

using Pagapoco.Core.Entities;

public interface IPublicationService
{
    /// Obtiene una publicación por su ID.
    Task<Publication?> GetByIdAsync(Guid id);

    /// Filtra publicaciones por ciudad y/o tipo de publicación.
    Task<IEnumerable<Publication>> SearchAsync(string? city, string? category);

    /// Crea una nueva publicación.
    Task CreateAsync(Publication publication);

    /// Edita una publicación existente.
    Task UpdateAsync(Publication publication);

    /// Elimina permanentemente una publicación.
    Task DeleteAsync(Guid publicationId);

    /// Pausa una publicación sin eliminarla.
    Task PauseAsync(Guid publicationId);

    /// Pausa o reactiva una publicación.
    Task SetPauseStateAsync(Guid publicationId, bool isPaused);
}
