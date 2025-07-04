using System;

namespace Pagapoco.Web.MVC.Models
{
    public class VehicleViewModel
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
        public string Transmission { get; set; } = null!;
        public string EngineDisplacement { get; set; } = null!;
        public string Version { get; set; } = null!;
        public int Doors { get; set; }
        public string FuelType { get; set; } = null!;
        public int KilometersDriven { get; set; }
        public string? ImageUrl { get; set; }
    }
}