using Pagapoco.Core.Entities;

namespace Pagapoco.Core.Interfaces;

public interface IImageService
{
    Task<IEnumerable<Image>> GetByPublicationIdAsync(Guid publicationId);
    Task AddImageAsync(Guid publicationId, Image image);
    Task RemoveAsync(Guid imageId);
}
