using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ExamProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Advertisement> Advertisements { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}