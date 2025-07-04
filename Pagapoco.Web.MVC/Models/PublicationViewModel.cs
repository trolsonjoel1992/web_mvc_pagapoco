using System;

namespace Pagapoco.Web.MVC.Models
{
    public class PublicationViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string City { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        // Puedes agregar más campos según lo que quieras mostrar en la vista
    }
}
