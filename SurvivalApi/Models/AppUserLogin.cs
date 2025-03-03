using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class AppUserLogin : IdentityUserLogin<string>
{
	public virtual AppUser User { get; set; }
}