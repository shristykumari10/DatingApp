using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    

    public DateOnly DateOfBirth { get; set; }

    public required string KnownAs { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;


    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public required string Gender { get; set; }
    public  string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public required string city { get; set; }
    public required string country { get; set; }
    public List<Photo> Photos { get; set; } = [];
    public List<UserLike> LikedByUser { get; set; } = [];
    public List<UserLike> LikedUsers { get; set; } = [];
    public List<Message> MessageSent { get; set; } = [];
    public List<Message> MessageReceived { get; set; } = [];

    public ICollection<AppUserRole> UserRoles { get; set; } = [];

    //public int GetAge()
    //{
    //    return DateOfBirth.CalculateAge();
    //}
    
}
