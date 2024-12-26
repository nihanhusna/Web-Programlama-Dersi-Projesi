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

            modelBuilder.Entity<ExpertOfTask>()
                .HasOne(e => e.RankTask)
                .WithMany(r => r.ExpertOfTasks)
                .HasForeignKey(e => e.RankTaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Meeting>()
               .HasOne(e => e.Personnel)
               .WithMany(r => r.Meetings)
               .HasForeignKey(e => e.PersonnelId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Meeting>()
              .HasOne(e => e.Identity_User)
              .WithMany(r => r.Meetings)
              .HasForeignKey(e => e.Identity_UserID)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Meeting>()
              .HasOne(e => e.RankTask)
              .WithMany(r => r.Meetings)
              .HasForeignKey(e => e.RankTaskId)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PersonnelShift>()
            .HasOne(ps => ps.Personnel)
            .WithOne(p => p.PersonnelShift)
            .HasForeignKey<PersonnelShift>(ps => ps.PersonnelId);

            modelBuilder.Entity<PersonnelShift>()
            .HasKey(ps => ps.PersonnelId);

        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ExpertOfTask> ExpertOfTasks { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<RankTask> RankTasks { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<PersonnelShift> PersonnelShifts { get; set; }
    }
}
