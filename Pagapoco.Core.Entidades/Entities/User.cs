namespace Pagapoco.Core.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string City { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;


    // Relaciones de navegación
    public ICollection<Publication> Publications { get; set; } = new List<Publication>();
    public ICollection<Question> Queries { get; set; } = new List<Question>();
    public ICollection<Answer> Responses { get; set; } = new List<Answer>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

}
