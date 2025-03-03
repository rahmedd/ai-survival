using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using api.Models;

namespace api.Data;

public class AppDbContext : IdentityDbContext<
	AppUser, AppRole, string,
	AppUserClaim, AppUserRole, AppUserLogin,
	AppRoleClaim, AppUserToken>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		// Customize the ASP.NET Identity model and override the defaults if needed.
		// For example, you can rename the ASP.NET Identity table names and more.
		// Add your customizations after calling base.OnModelCreating(builder);

		builder.Entity<AppUser>(b =>
		{
			// Each User can have many UserClaims
			b.HasMany(e => e.Claims)
				.WithOne(e => e.User)
				.HasForeignKey(uc => uc.UserId)
				.IsRequired();

			// Each User can have many UserLogins
			b.HasMany(e => e.Logins)
				.WithOne(e => e.User)
				.HasForeignKey(ul => ul.UserId)
				.IsRequired();

			// Each User can have many UserTokens
			b.HasMany(e => e.Tokens)
				.WithOne(e => e.User)
				.HasForeignKey(ut => ut.UserId)
				.IsRequired();

			// Each User can have many entries in the UserRole join table
			b.HasMany(e => e.UserRoles)
				.WithOne(e => e.User)
				.HasForeignKey(ur => ur.UserId)
				.IsRequired();
		});

		builder.Entity<AppRole>(b =>
		{
			// Each Role can have many entries in the UserRole join table
			b.HasMany(e => e.UserRoles)
				.WithOne(e => e.Role)
				.HasForeignKey(ur => ur.RoleId)
				.IsRequired();

			// Each Role can have many associated RoleClaims
			b.HasMany(e => e.RoleClaims)
				.WithOne(e => e.Role)
				.HasForeignKey(rc => rc.RoleId)
				.IsRequired();
		});
	}

	// public DbSet<AppRole> AppRoles { get; set; } = null!;
	// public DbSet<AppRoleClaim> AppRoleClaims { get; set; } = null!;
	// public DbSet<AppUserClaim> AppUserClaims { get; set; } = null!;
	// public DbSet<AppUserLogin> AppUserLogins { get; set; } = null!;
	// public DbSet<AppUserToken> AppUserTokens { get; set; } = null!;
	// public DbSet<Job> Jobs { get; set; } = null!;
}
