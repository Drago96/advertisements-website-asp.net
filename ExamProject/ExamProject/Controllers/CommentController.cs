using ExamProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ExamProject.Controllers
{
    public class CommentController : Controller
    {
        //POST: CommentCreate
        [Authorize]
        [HttpPost]
        public ActionResult Create(string content, string authorName, string targetName)
        {
            if(authorName == null || targetName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(content == null || content == "")
            {
                return RedirectToAction("Details", "Account", new { name = targetName });
            }

            using (var database = new ApplicationDbContext())
            {
                var author = database.Users.Where(u => u.Email == authorName).First();
                var target = database.Users.Where(u => u.Email == targetName).First();

  

                var comment = new Comment(content, author.Id, target.Id);

                database.Comments.Add(comment);

                author.WrittenComments.Add(comment);
              
                target.ProfileComments.Add(comment);
                
                

                database.SaveChanges();

                return RedirectToAction("Details", "Account", new { name = target.Email });
            }

            
        }
    }
}