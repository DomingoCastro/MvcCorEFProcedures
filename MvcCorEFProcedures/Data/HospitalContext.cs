using Microsoft.EntityFrameworkCore;
using MvcCorEFProcedures.Models;

namespace MvcCorEFProcedures.Data
{
    public class HospitalContext: DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) 
        :base(options) { }

        public DbSet<Enfermo> Enfermos { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
    }
}
