using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ESportsTeams.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<TeamTournament>()
                .HasKey(tt => new { tt.TeamId, tt.TournamentId });

            builder.Entity<Team>()
                 .HasOne(t => t.Owner)
                 .WithMany(t => t.OwnedTeams)
                 .HasForeignKey(t => t.OwnerId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Team>()
                .HasIndex(t => t.Name)
                .IsUnique();

            builder.Entity<Team>()
               .HasMany(t => t.Requests)
               .WithOne(t => t.Team)
               .HasForeignKey(t => t.TeamId)
               .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }


        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Request> Requests { get; set; } = null!;
        public DbSet<Tournament> Tournaments { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
    }
}