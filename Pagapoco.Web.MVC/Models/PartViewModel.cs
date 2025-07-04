using System;

namespace Pagapoco.Web.MVC.Models
{
    public class PartViewModel
    {
        public Guid? Id { get; set; } // Opcional, útil para edición
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string City { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!; // Modelo compatible o referencia
        public string Condition { get; set; } = null!; // Nuevo, usado, reacondicionado, etc.
        public string? Compatibility { get; set; } // Ej: modelos o marcas compatibles
        public string? ImageUrl { get; set; }
    }
}