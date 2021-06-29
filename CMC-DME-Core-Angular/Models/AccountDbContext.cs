using Microsoft.EntityFrameworkCore;

namespace CMC_DME_Core_Angular.Models
{
  public class AccountDbContext:DbContext
  {
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    { }

    public DbSet<Account> AccountDetails { get; set; }
  }
}
