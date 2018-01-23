using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoPlayer.Model;
using VideoPlayer.Models;

namespace VideoPlayer.DAL
{
    public partial class VideoManagerDbContext : IdentityDbContext<ApplicationUser>
    {
        public VideoManagerDbContext()
            : base()
        {
        }

        public VideoManagerDbContext(DbContextOptions<VideoManagerDbContext> context) : base(context) { }

        public static VideoManagerDbContext Create()
        {
            return new VideoManagerDbContext();
        }

        public DbSet<Series> Series { get; set; }
        public DbSet<Cartoon> Cartoons { get; set; }
        public DbSet<Film> Films { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VIDEOSDB_065609c8d3e242a1acad3edf3349292b;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

    
}
