namespace Pagapoco.API.Dtos
{
    public class ImageCreateDto
    {
        public string Url { get; set; } = null!;
        public string? AltText { get; set; }
        public int? DisplayOrder { get; set; }
    }
}