namespace Pagapoco.API.Dtos
{
    public class PublicationReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string City { get; set; } = null!;
        public bool IsPremium { get; set; }
        public string Type { get; set; } = null!;
        public Guid UserId { get; set; }

        // Campos específicos de repuesto
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Condition { get; set; } = null!;
        public string Compatibility { get; set; } = null!;

        // Opcional: lista de URLs de imágenes
        public List<string>? ImageUrls { get; set; }
    }
}