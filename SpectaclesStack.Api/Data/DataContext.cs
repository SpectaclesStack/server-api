using Microsoft.EntityFrameworkCore;
using spectaclesStackServer.Model;

namespace spectaclesStackServer.Data
{

    public class DataContext: DbContext
    {

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    
    }
    public DbSet<Users> Users { get; set; } = default!;

    public DbSet<Questions> Questions { get; set; } = default!;

    public DbSet<Answers> Answers { get; set; } = default!;
    
    }
    
}

