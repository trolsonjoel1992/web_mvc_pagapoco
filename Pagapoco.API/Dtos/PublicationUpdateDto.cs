namespace Pagapoco.API.Dtos
{
    public class PublicationUpdateDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string City { get; set; } = null!;
        public bool IsPremium { get; set; }
        // Puedes agregar aquí otros campos editables si lo necesitas
    }
}