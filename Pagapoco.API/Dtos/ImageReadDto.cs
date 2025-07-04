namespace Pagapoco.API.Dtos
{
    public class ImageReadDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string? AltText { get; set; }
        public int? DisplayOrder { get; set; }
    }
}