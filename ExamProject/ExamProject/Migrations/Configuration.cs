namespace ExamProject.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<ExamProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "ExamProject.Models.ApplicationDbContext";
        }

        protected override void Seed(ExamProject.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                this.CreateRole(context, "Admin");
                this.CreateRole(context, "User");
            }

            if (!context.Users.Any())
            {
                this.CreateNewUser(context, "admin@gmail.com", "Dragomir Proychev", "asdasd", "0000000000", "Male", "01/01/1990");
                this.SetRoleToUser(context, "admin@gmail.com", "Admin");
                this.SetRoleToUser(context, "admin@gmail.com", "User");
            }
        }

        private void SetRoleToUser(ApplicationDbContext context, string username, string role)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var user = context.Users.Where(u => u.UserName == username).First();

            var result = userManager.AddToRole(user.Id, role);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateNewUser(ApplicationDbContext context, string email, string fullName, string password, string phoneNumber, string gender, string birthday)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false
            };

            var admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                PhoneNumber = phoneNumber,
                Gender = gender,
                Birthday = birthday
            };

            var result = userManager.Create(admin, password);
        }

        private void CreateRole(ApplicationDbContext database, string role)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(database));

            var result = roleManager.Create(new IdentityRole(role));

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }
    }
}