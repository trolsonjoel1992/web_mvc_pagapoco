using System;
using System.ComponentModel.DataAnnotations;

namespace Pagapoco.Web.MVC.Models
{
    public class UserEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string? Name { get; set; }

        [Phone(ErrorMessage = "El teléfono no es válido")]
        public string? Phone { get; set; }

        public string? City { get; set; }
    }
}