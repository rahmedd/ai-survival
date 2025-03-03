using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class AppUserToken : IdentityUserToken<string>
{
	public virtual AppUser User { get; set; }
}