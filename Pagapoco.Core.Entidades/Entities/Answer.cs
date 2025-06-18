namespace Pagapoco.Core.Entities;

public class Answer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
