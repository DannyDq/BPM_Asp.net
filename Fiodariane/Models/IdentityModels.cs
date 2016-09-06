using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Fiodariane.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Fiodariane.Models.ProcessModels> ProcessModels { get; set; }

        public System.Data.Entity.DbSet<Fiodariane.Models.Procedure> Procedures { get; set; }

        public System.Data.Entity.DbSet<Fiodariane.Models.Manager> Managers { get; set; }

        public System.Data.Entity.DbSet<Fiodariane.Models.Entity> Entities { get; set; }

        public System.Data.Entity.DbSet<Fiodariane.Models.TaskEntity> TaskEntities { get; set; }

        public System.Data.Entity.DbSet<Fiodariane.Models.HRSkills> HRSkills { get; set; }

        public System.Data.Entity.DbSet<Fiodariane.Models.SupportingIS> SupportingIS { get; set; }

        public System.Data.Entity.DbSet<Fiodariane.Models.Tasks> Tasks { get; set; }

        public System.Data.Entity.DbSet<Fiodariane.Models.SysUrl> SysUrls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}