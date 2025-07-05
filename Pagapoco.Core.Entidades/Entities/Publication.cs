using System;

namespace Pagapoco.Core.Entities
{
    public class Publication
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string City { get; set; } = null!;
    public bool IsPremium { get; set; }
    public string Type { get; set; } = null!;
    public bool IsPaused { get; set; } = false;
    public Guid UserId { get; set; }
    

        // Campos específicos de Part
        public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Condition { get; set; } = null!;
    public string Compatibility { get; set; } = null!;
    // Relación con imágenes
    public ICollection<Image> Images { get; set; } = new List<Image>();

}
}

//public abstract class Publication
//{
//    public Guid Id { get; set; } = Guid.NewGuid();
//    public string Title { get; set; } = null!;
//    public string Description { get; set; } = null!;
//    public decimal Price { get; set; }
//    public string City { get; set; } = null!;
//    public bool IsPremium { get; set; }
//    public string Type { get; set; } = null!;
//    public bool IsPaused { get; set; } = false;

//    // Relación con el usuario que publica
//    public Guid UserId { get; set; }
//    public User User { get; set; } = null!;

//    // Navegación hacia imágenes y consultas

