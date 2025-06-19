using Pagapoco.Core.Entities;
using Pagapoco.Core.Interfaces;
using System;

namespace Pagapoco.Application.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;

    public NotificationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        => await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var noti = await _context.Notifications.FindAsync(notificationId);
        if (noti != null)
        {
            noti.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task NotifyQuestionAsync(Question question)
    {
        var noti = new Notification
        {
            UserId = question.Publication.UserId,
            Type = "Pregunta",
            Content = $"Nuevo comentario en tu publicación: {question.Content}",
            PublicationId = question.PublicationId,
            QuestionId = question.Id
        };

        _context.Notifications.Add(noti);
        await _context.SaveChangesAsync();
    }

    public async Task NotifyAnswerAsync(Answer answer)
    {
        var question = await _context.Questions.FindAsync(answer.QuestionId);
        if (question == null) return;

        var noti = new Notification
        {
            UserId = question.UserId,
            Type = "Respuesta",
            Content = $"Te respondieron: {answer.Content}",
            PublicationId = question.PublicationId,
            QuestionId = question.Id
        };

        _context.Notifications.Add(noti);
        await _context.SaveChangesAsync();
    }
}
