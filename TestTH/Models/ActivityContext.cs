using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestTH.Authentication;

namespace TestTH.Models
{
    public class ActivityContext: IdentityDbContext<ApplicationUser>
    {
        public ActivityContext(DbContextOptions<ActivityContext> options) : base(options)
        {

        }

        public DbSet<Activity> Activity { get; set; }
        public DbSet<Property> Property { get; set; }
        public DbSet<Survey> Survey { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }

}
