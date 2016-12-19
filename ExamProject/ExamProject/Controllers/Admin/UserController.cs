using ExamProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ExamProject.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //GET: User/List
        public ActionResult List()
        {
            using (var database = new ApplicationDbContext())
            {
                var users = database.Users.ToList();

                var admins = GetAdminUserNames(users, database);
                ViewBag.admins = admins;

                return View(users);
            }
        }

       
        //GET: User/Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var user = database.Users.Where(u => u.Id.Equals(id)).First();

                if (user == null)
                {
                    return HttpNotFound();
                }

                if (this.User.Identity.Name == user.Email)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                return View(user);


            }
        }

        //POST: User/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var user = database.Users.Where(u => u.Id == id).First();

                if (user == null)
                {
                    return HttpNotFound();
                }

                if (this.User.Identity.Name == user.Email)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }



                foreach (var advertisement in user.Advertisements.ToList())
                {
                    var fullPath = Server.MapPath("~") + advertisement.ImageUrl.Substring(1);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    var directory = new DirectoryInfo(Server.MapPath("~") + "uploads/" + advertisement.Id);
                    if (directory.Exists)
                    {
                        directory.Delete(true);
                    }
                    database.Advertisements.Remove(advertisement);
                }

                foreach (var comment in user.ProfileComments.ToList())
                {
                    database.Comments.Remove(comment);
                }

                foreach (var comment in user.WrittenComments.ToList())
                {
                    database.Comments.Remove(comment);
                }



                database.Users.Remove(user);
                database.SaveChanges();
                return RedirectToAction("List", "User");

            }
        }

        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, ApplicationDbContext database)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(database));

            var admins = new HashSet<string>();

            foreach(var user in users)
            {
                if(userManager.IsInRole(user.Id,"Admin"))
                {
                    admins.Add(user.UserName);
                }
            }

            return admins;
        }
    }
}