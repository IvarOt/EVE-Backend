using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace eve_backend.data
{
	public class ApplicationDbContext : DbContext
	{
		
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options) { }
	}
}
