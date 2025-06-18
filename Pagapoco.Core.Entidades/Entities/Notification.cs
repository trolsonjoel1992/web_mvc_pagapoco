namespace Pagapoco.Core.Entities;

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;

    // Usuario que recibe la notificación
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // Tipo opcional: "QuestionReceived", "AnswerReceived", etc.
    public string Type { get; set; } = null!;

    // ID opcional de la publicación o pregunta relacionada
    public Guid? PublicationId { get; set; }
    public Guid? QuestionId { get; set; }
}
