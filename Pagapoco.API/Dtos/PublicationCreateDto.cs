using System;

namespace Pagapoco.API.Dtos
{
    public class PublicationCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string City { get; set; }
        public bool IsPremium { get; set; }
        public string Type { get; set; }
        public Guid UserId { get; set; }

        // Vehicle y Bike
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public string? Color { get; set; }
        public string? FuelType { get; set; }
        public string? Transmission { get; set; }
        public string? EngineDisplacement { get; set; }
        public int? KilometersDriven { get; set; }

        // Vehicle
        public string? Version { get; set; }
        public int? Doors { get; set; }

        // Bike
        public int? EnginePower { get; set; }
        public string? WheelSize { get; set; }

        // Part
        public string? Condition { get; set; }
        public string? Compatibility { get; set; }
    }
}
