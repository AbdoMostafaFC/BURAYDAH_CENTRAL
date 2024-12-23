using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BURAYDAH_CENTRAL.Models
{
    public class ApplicationDBContext:IdentityDbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions):base(dbContextOptions)
        {
            
        }

        public DbSet<Patient> Patients { get; set; }
        public   DbSet<Pathology_analysis> PathologyAnalysis { get; set; }
    }
}
