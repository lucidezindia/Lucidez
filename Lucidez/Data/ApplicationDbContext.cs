using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lucidez.Data
{
    // Add IdentityDbContext so Identity tables are created in same DB
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // existing DbSets
        public DbSet<Models.Lead> Leads { get; set; }
        public DbSet<Models.ContactMessage> ContactMessages { get; set; }
        public DbSet<Models.ChatMessage> ChatMessages { get; set; }
    }
}
