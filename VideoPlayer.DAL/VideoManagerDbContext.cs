using Microsoft.EntityFrameworkCore;
using VideoPlayer.Model;

namespace VideoPlayer.DAL
{
    public partial class VideoManagerDbContext : DbContext
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VIDEOSDB_065609c8d3e242a1acad3edf3349292b;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
    }

    
}
