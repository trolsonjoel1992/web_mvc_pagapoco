namespace Pagapoco.Core.Interfaces;

using Pagapoco.Core.Entities;

public interface IUserService
{
    /// Obtiene un usuario por su email.
    Task<User?> GetByEmailAsync(string email);

    /// Editar un usuario
    Task UpdateAsync(User user);

    /// Obtiene un usuario por su ID único.
    Task<User?> GetByIdAsync(Guid id);

    /// Elimina (lógicamente) un usuario por su ID.
    Task DeleteByIdAsync(Guid userId, bool softDelete = true);

    /// Elimina (lógicamente) un usuario por su Email.
    Task DeleteByEmailAsync(string email, bool softDelete = true);

    /// Devuelve todas las publicaciones creadas por un usuario.
    Task<IEnumerable<Publication>> GetUserPublicationsAsync(Guid userId);

    /// Retorna las notificaciones vinculadas a un usuario.
    
}
