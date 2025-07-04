using System;

namespace Pagapoco.Web.MVC.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}