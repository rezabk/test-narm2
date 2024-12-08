using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.Profile;

public class UpdateUserViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public IFormFile? File { get; set; }
    public string? Birthday { get; set; }
}