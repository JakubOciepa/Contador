using Contador.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Contador.DAL.DbContext
{
    public class ContadorContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public ContadorContext(DbContextOptions<ContadorContext> options)
            : base(options) { }

        public DbSet<Expense> Expenses { get; set; }
    }
}
