namespace Pagapoco.Core.Entities;

public class Part : Publication
{
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Condition { get; set; } = null!;
    public string Compatibility { get; set; } = null!;
}
