namespace Pagapoco.Services.Interfaces;

using Pagapoco.Core.Entities;

public interface IPartService
{
    Part CreatePart(
        Guid userId,
        string title,
        string description,
        decimal price,
        string city,
        string brand,
        string model,
        string color,
        string condition,
        string compatibility
    );

    /// Busca repuestos compatibles con un campo específico
    List<Part> GetCompatibleParts(string? brand, string? model, string? color, string? condition, string? compatibility);
}