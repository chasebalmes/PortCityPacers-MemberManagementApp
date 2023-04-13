using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortCityPacers.Models;

namespace PortCityPacers.Data
{
    public class MembersContext : DbContext
    {
        public MembersContext (DbContextOptions<MembersContext> options)
            : base(options)
        {
        }

        public DbSet<PortCityPacers.Models.Member> Member { get; set; } = default!;
    }
}
