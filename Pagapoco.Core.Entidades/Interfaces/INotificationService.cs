using Pagapoco.Core.Entities;

namespace Pagapoco.Core.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
    Task MarkAsReadAsync(Guid notificationId);

    Task NotifyQuestionAsync(Question question);
    Task NotifyAnswerAsync(Answer answer);
}
