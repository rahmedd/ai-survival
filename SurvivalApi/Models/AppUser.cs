using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class AppUser : IdentityUser
{
	public virtual ICollection<AppUserClaim> Claims { get; set; }
	public virtual ICollection<AppUserLogin> Logins { get; set; }
	public virtual ICollection<AppUserToken> Tokens { get; set; }
	public virtual ICollection<AppUserRole> UserRoles { get; set; }
}