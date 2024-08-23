using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InforceTask.Models
{
	public class UrlDbContext : IdentityDbContext
	{
		public UrlDbContext(DbContextOptions options) : base(options) { }
		public DbSet<Url> Urls { get; set; }
	}
}
