using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Contador.Web.Server.Identity
{
	public class IdentityDatabaseContext : IdentityDbContext
	{
		public IdentityDatabaseContext(DbContextOptions options) : base(options)
		{
		}
	}
}
