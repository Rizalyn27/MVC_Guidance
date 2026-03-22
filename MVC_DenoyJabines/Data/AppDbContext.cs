using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines;
using MVC_DenoyJabines.Models;

namespace MVC_DenoyJabines.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<MVC_DenoyJabines.Models.Users> User { get; set; }
        public DbSet<MVC_DenoyJabines.Models.Students> Students { get; set; }

        public DbSet<MVC_DenoyJabines.Models.Appointment> Appointments { get; set; }


    }
}
