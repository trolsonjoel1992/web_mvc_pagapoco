using System;

namespace Pagapoco.Web.MVC.Models
{
    public class ImageViewModel
    {
        public Guid? Id { get; set; }
        public string Url { get; set; } = null!;
        public Guid PublicationId { get; set; }
        public string? Description { get; set; }
    }
}