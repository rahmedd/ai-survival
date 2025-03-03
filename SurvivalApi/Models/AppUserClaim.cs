using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class AppUserClaim : IdentityUserClaim<string>
{
	public virtual AppUser User { get; set; }
}