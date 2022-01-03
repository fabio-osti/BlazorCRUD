using BlazorCRUD.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorCRUD.Server
{
	public class PersonContext : DbContext
	{
		public PersonContext(DbContextOptions<PersonContext> options) : base(options)
		{ }

		public DbSet<Person>? Persons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Person>().HasKey(e => e.Id);

			modelBuilder.Entity<Person>()
				.Property(e => e.Age)
				.HasConversion<System.Byte>();

			modelBuilder.Entity<Person>()
				.Property(e => e.HairColor)
				.HasConversion(hc => (System.Byte)hc!, hc => (HairColor)hc);
		}
	}
}