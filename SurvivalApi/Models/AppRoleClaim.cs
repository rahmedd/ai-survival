using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class AppRoleClaim : IdentityRoleClaim<string>
{
	public virtual AppRole Role { get; set; }
}