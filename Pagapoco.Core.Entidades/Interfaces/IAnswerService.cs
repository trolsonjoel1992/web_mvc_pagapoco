using Pagapoco.Core.Entities;

namespace Pagapoco.Core.Interfaces;

public interface IAnswerService
{
    Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId);
    Task CreateAsync(Answer answer);
}
