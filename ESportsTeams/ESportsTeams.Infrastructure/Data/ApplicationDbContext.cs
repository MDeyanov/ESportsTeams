using ESportsTeams.Infrastructure.Data.Entity;
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

            //builder.Entity<AppUser>()
            //    .HasOne(ap => ap.Team)
            //    .WithMany(ap => ap.AppUsers)
            //    .HasForeignKey(ap => ap.Id)
            //    .OnDelete(DeleteBehavior.Cascade);         

            //builder.Entity<Address>()
            //    .HasMany(a => a.Tournaments)
            //    .WithOne(a => a.Address)
            //    .HasForeignKey(a => a.AddressId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Address>()
            //.HasMany(a => a.Teams)
            //.WithOne(a => a.Address)
            //.HasForeignKey(a => a.AddressId)
            //.OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<TeamTournament>()
            //    .HasOne(t => t.Team)
            //    .WithMany(t => t.TeamsTournaments)
            //    .HasForeignKey(t => t.TeamId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<TeamTournament>()
            //     .HasOne(t => t.Tournament)
            //     .WithMany(t => t.TeamsTournaments)
            //     .HasForeignKey(t => t.TournamentId)
            //     .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Tournament>()
            //   .HasMany<TeamTournament>()
            //   .WithOne(t => t.Tournament)
            //   .HasForeignKey(t => t.TournamentId)
            //   .OnDelete(DeleteBehavior.Cascade);


            //builder.Entity<Blog>()
            //    .HasMany(b => b.Comments)
            //    .WithOne(b => b.Blog)
            //    .HasForeignKey(b => b.BlogId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Comment>()
            //    .HasOne(c => c.Blog)
            //    .WithMany(c => c.Comments)
            //    .HasForeignKey(c => c.BlogId)
            //    .OnDelete(DeleteBehavior.Cascade);



            base.OnModelCreating(builder);
        }
      

        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Tournament> Tournaments { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Review> Comments { get; set; } = null!;
    }
}