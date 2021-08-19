using ClarityConsole.Models;
using ClarityMailLibrary;
using Microsoft.EntityFrameworkCore;

namespace ClarityConsole.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<ClarityMail> Emails { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
    }
}