using BlazorCRUD.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorCRUD.Server
{
	public class UserContext : DbContext
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options) { }

		public DbSet<User>? Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			var user = modelBuilder.Entity<User>();

			user.Metadata.SetTableName("USERS");
			user.HasIndex(e => e.Email).IsUnique();
			user.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(32).IsRequired();
			user.Property(e => e.Username).HasColumnName("USERNAME").HasMaxLength(16).IsRequired();
			user.Property(e => e.Password).HasColumnName("PASSWORD").HasMaxLength(64).IsRequired();
			user.Property(e => e.Salt).HasColumnName("SALT").HasMaxLength(32).IsRequired();
		}
	}
}
