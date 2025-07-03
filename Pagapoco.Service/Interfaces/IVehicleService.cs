namespace Pagapoco.Services.Interfaces;

using Pagapoco.Core.Entities;

/// <summary>
/// Contrato para servicios específicos de vehículos
/// </summary>
public interface IVehicleService
{
    /// Crea una publicación de tipo Vehicle con datos específicos
    Vehicle CreateVehicle(
        Guid userId,
        string title,
        string description,
        decimal price,
        string city,
        string brand,
        string model,
        int year,
        string Color,
        string transmission,
        string engineDisplacement,
        string version,
        int doors,
        string fuelType,
        int kilometersDriven,
        string? imageUrl
    );
    /// Busca vehículos por campos específicos
    List<Vehicle> SearchVehicles(string? brand, string? model, int? year, string? Color, string? transmission, string? engineDisplacement, string? version, int? doors, string? fuelType, int? kilometersDriven);
}