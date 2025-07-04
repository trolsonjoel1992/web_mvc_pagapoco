using System;

namespace Pagapoco.Web.MVC.Models
{
    public class BikeViewModel
    {
        public Guid? Id { get; set; } 
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string City { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string Color { get; set; } = null!;
        public string Type { get; set; } = null!; // Ej: montaña, ruta, urbana
        public string FrameMaterial { get; set; } = null!; // Ej: aluminio, carbono
        public int WheelSize { get; set; } // En pulgadas
        public string? ImageUrl { get; set; }
    }
}