using Pagapoco.Web.MVC.Models;

public class UserPublicationViewModel
{
    public PublicationViewModel Publication { get; set; } = null!;
    public string? MainImageUrl { get; set; }
}