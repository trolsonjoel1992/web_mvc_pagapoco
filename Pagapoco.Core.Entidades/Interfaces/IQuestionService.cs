using Pagapoco.Core.Entities;

namespace Pagapoco.Core.Interfaces;

public interface IQuestionService
{
    Task<Question?> GetByIdAsync(Guid id);
    Task<IEnumerable<Question>> GetByPublicationIdAsync(Guid publicationId);
    Task<IEnumerable<Question>> GetByUserIdAsync(Guid userId);
    Task CreateAsync(Question question);
}
