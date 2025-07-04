namespace Pagapoco.Services.Interfaces;

using Pagapoco.Core.Entities;

/// Contrato para servicios relacionados con usuarios
public interface IUserService
{
    /// Registra un nuevo usuario en el sistema
    User Register(string email, string password, string name, string phone, string city);
    /// Valida las credenciales y retorna el usuario si son correctas
    User? Login(string email, string password);
    /// Actualiza los datos básicos del usuario (nombre, teléfono, ciudad)
    void UpdateUser(Guid userId, string name, string phone, string city);
    /// Elimina (lógica o físicamente) un usuario por su ID
    void DeleteUser(Guid userId, bool softDelete = true);
    /// Obtiene todas las publicaciones creadas por un usuario    
    List<Publication> GetUserPublications(Guid userId);
}


//public interface IUserService
//{
//    /// Obtiene un usuario por su email.
//    Task<User?> GetByEmailAsync(string email);
//    /// Editar un usuario
//    Task UpdateAsync(User user);
//    /// Obtiene un usuario por su ID único.
//    Task<User?> GetByIdAsync(Guid id);
//    /// Elimina (lógicamente) un usuario por su ID.
//    Task DeleteByIdAsync(Guid userId, bool softDelete = true);
//    /// Elimina (lógicamente) un usuario por su Email.
//    Task DeleteByEmailAsync(string email, bool softDelete = true);
//    /// Devuelve todas las publicaciones creadas por un usuario.
//    Task<IEnumerable<Publication>> GetUserPublicationsAsync(Guid userId);
//    /// Retorna las notificaciones vinculadas a un usuario.
//}