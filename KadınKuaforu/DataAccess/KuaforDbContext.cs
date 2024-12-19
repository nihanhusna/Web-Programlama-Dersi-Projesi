using KadınKuaforu.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.DataAccess
{
    public class KuaforDbContext : IdentityDbContext<Identity_User, Identity_Role, string>
    {
        public KuaforDbContext(DbContextOptions<KuaforDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
        public DbSet<Company> Companies { get; set; }
    }
}
