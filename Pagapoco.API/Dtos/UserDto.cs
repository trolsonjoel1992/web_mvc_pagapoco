namespace Pagapoco.API.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? City { get; set; }
    }
}