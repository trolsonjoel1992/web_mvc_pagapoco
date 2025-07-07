using System.ComponentModel.DataAnnotations.Schema;

namespace Pagapoco.Core.Entities;


[Table("Images")]
public class Image
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = null!;
    public string? AltText { get; set; }
    public int? DisplayOrder { get; set; }

    public Guid PublicationId { get; set; }
    public Publication Publication { get; set; } = null!;
}
