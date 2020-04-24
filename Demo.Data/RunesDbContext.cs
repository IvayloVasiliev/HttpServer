namespace IRunes.Data
{
    using Microsoft.EntityFrameworkCore;

    using IRunes.Models;
    
    public class RunesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Album> Albums { get; set; }

        public RunesDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .HasKey(user => user.Id);

            modelBuilder.Entity<User>()
                .Property(user => user.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Track>()
             .Property(track => track.Id)
             .ValueGeneratedOnAdd();

            modelBuilder.Entity<Album>()
             .Property(album => album.Id)
             .ValueGeneratedOnAdd();

            modelBuilder.Entity<Album>()
                .HasMany(album => album.Tracks);

            base.OnModelCreating(modelBuilder);
        }

    }
}
