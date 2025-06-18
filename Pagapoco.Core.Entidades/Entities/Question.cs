namespace Pagapoco.Core.Entities;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;           // Quien hace la pregunta
    public string Phone { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid PublicationId { get; set; }
    public Publication Publication { get; set; } = null!;

    public Guid? UserId { get; set; }                   // Puede ser null si es anónimo
    public User? User { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
