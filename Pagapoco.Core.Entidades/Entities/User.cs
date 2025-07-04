namespace Pagapoco.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public bool IsDeleted { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;

    //public Guid Id { get; set; } = Guid.NewGuid();

    //public string Name { get; set; } = null!;

    //public string Email { get; set; } = null!;
    //public string Phone { get; set; } = null!;
    //public string City { get; set; } = null!;
    //public bool IsDeleted { get; set; } = false;


    // Relaciones de navegación
    public ICollection<Publication> Publications { get; set; } = new List<Publication>();
    
}
