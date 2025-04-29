using Website.Helpers;

namespace Website.Models.ViewModels;

public class HomeViewModel
{
    public PaginatedList<Review> Reviews { get; set; }
}