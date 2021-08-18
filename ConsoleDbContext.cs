using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClarityConsole
{
    public class ConsoleDbContext : DbContext
    {
        public ConsoleDbContext(DbContextOptions<ConsoleDbContext> options)
            : base(options)
        {
        }
    }
}
