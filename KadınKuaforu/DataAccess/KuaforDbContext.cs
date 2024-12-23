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
            modelBuilder.Entity<ExpertOfTask>()
                .HasOne(e => e.Personnel)
                .WithMany(p => p.ExpertOfTasks)
                .HasForeignKey(e => e.PersonnelId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExpertOfTask>()
                .HasOne(e => e.RankTask)
                .WithMany(r => r.ExpertOfTasks)
                .HasForeignKey(e => e.RankTaskId)
                .OnDelete(DeleteBehavior.NoAction);

        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ExpertOfTask> ExpertOfTasks { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<RankTask> RankTasks { get; set; }
    }
}
