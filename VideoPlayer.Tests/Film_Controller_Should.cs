using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoPlayer.DAL;
using Xunit;

namespace VideoPlayer.Tests
{
    public class Film_Controller_Should
    {
        DbContextOptions<VideoManagerDbContext> _dbContextOptions;

        public Film_Controller_Should()
        {
            _dbContextOptions = new DbContextOptionsBuilder<VideoManagerDbContext>()
                            .UseInMemoryDatabase(databaseName: "Test_database")
                            .Options;
        }
        [Fact]
        public async void Test()
        {

        }
    }
}
