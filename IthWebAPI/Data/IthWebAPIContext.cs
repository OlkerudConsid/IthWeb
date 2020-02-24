using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IthWebAPI.Models;

namespace IthWebAPI.Data
{
    public class IthWebAPIContext : DbContext
    {
        public IthWebAPIContext (DbContextOptions<IthWebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<IthWebAPI.Models.BlogPost> BlogPost { get; set; }
    }
}
