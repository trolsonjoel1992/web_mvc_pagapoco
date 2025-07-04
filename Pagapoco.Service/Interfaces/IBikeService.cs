namespace Pagapoco.Services.Interfaces;

using Pagapoco.Core.Entities;

public interface IBikeService
{
    Bike CreateBike(
        Guid userId,
        string title,
        string description,
        decimal price,
        string city,
        string brand,
        string model,
        int year,
        string color,
        string transmission,
        string frameSize,
        int kilometersDriven,
        int enginePower,
        string wheelSize,
        string imageUrl
    );


    /// Busca motos por campo específico.
    List<Bike> GetBikesByWheelSize(string? brand, string? model, int? year, string? color, string? transmission, string? frameSize, int? kilometersDriven, int? enginePower, string? wheelSize, string? imageUrl);
}