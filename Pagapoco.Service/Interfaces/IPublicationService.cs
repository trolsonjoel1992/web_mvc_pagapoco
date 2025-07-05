using Pagapoco.Core.Entities;

namespace Pagapoco.Services.Interfaces
{
    public interface IPublicationService
    {
        // Crear publicación
        Publication CreatePublication(Publication publication, Guid userId);

        // Obtener publicación por ID (con opción de incluir imágenes)
        Publication? GetPublicationById(Guid id, bool includeImages = true);

        // Paginado
        (List<Publication> publications, int total) GetPublicationsPaginated(int page, int pageSize);

        // Obtener todas las publicaciones
        List<Publication> GetAllPublications();

        // Buscar publicaciones por ciudad y tipo
        List<Publication> SearchPublications(string? city, string? publicationType);

        // Actualizar publicación
        void UpdatePublication(Guid publicationId, Guid userId, string title, string description, decimal price);

        // Eliminar publicación (borrado lógico o físico)
        void DeletePublication(Guid publicationId, Guid userId, bool softDelete = true);

        // Publicaciones de un usuario
        List<Publication> GetUserPublications(Guid userId);

        // Filtrar publicaciones por campos de partes
        List<Publication> FilterPublications(string type, Dictionary<string, object> filters);

        // Pausar publicación
        void PausePublication(Guid publicationId);

        // Activar publicación
        void ActivatePublication(Guid publicationId);

        // Alternativa genérica para pausar/activar
        void SetPauseState(Guid publicationId, bool isPaused);
    }
}