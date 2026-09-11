using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public int? WorkplaceId { get; set; }
    public string? EmployeeType { get; set; }
}
