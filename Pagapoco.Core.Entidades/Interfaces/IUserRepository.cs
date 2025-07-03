namespace Pagapoco.Core.Interfaces;

using Pagapoco.Core.Entities;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);          // Para registro
    Task UpdateAsync(User user);       // Para edición
    Task DeleteAsync(User user, bool softDelete = true); // Para eliminación (lógica o física)
    Task<bool> EmailExistsAsync(string email); // Validación extra
}