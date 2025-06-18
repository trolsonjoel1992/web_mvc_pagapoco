namespace Pagapoco.Core.Entities;

public class Bike : Publication
{
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string Color { get; set; } = null!;
    public string FuelType { get; set; } = null!;
    public string Transmission { get; set; } = null!;
    public string EngineDisplacement { get; set; } = null!;
    public int KilometersDriven { get; set; }
    public string WheelSize { get; set; } = null!; 
}
