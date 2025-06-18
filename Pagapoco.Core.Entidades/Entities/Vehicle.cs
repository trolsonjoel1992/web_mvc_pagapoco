namespace Pagapoco.Core.Entities;

public class Vehicle : Publication
{
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string Color { get; set; } = null!;
    public string FuelType { get; set; } = null!;
    public string Transmission { get; set; } = null!;
    public string EngineDisplacement { get; set; } = null!;
    public int KilometersDriven { get; set; }
    public string Version { get; set; } = null!;
    public int Doors { get; set; }
}
