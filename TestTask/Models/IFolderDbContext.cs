using Microsoft.EntityFrameworkCore;

namespace TestTask.Models
{
    public interface IFolderDbContext
    {
        DbSet<Folder> folders { get; set; }

        Task<int> SaveChangesAsync();
    }
}
