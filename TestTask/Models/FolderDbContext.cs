using Microsoft.EntityFrameworkCore;

namespace TestTask.Models
{
    public class FolderDbContext : DbContext, IFolderDbContext
    {
        public FolderDbContext(DbContextOptions op): base(op) 
        {
            Database.EnsureCreated();
    
        }

       public DbSet<Folder> folders { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
