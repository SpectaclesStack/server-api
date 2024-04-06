using Microsoft.EntityFrameworkCore;
using stackup_vsc_setup.Model;

namespace stackup_vsc_setup.Data
{

    public class DataContext: DbContext
    {

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    
    }
    public DbSet<Users> Users { get; set; }

    public DbSet<Questions> Questions { get; set; }

    public DbSet<Answers> Answers { get; set; }
    
    }
    
}

