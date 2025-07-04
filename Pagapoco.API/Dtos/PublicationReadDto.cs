using System;
using System.Collections.Generic;

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
        public bool IsPaused { get; set; }
        public Guid UserId { get; set; }
        public List<ImageDto> Images { get; set; } = new();
    }

    public class ImageDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string? AltText { get; set; }
        public int? DisplayOrder { get; set; }
    }
}