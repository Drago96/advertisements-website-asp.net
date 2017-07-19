using ExamProject.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            if (authorName == null || targetName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrEmpty(content))
            {
                return RedirectToAction("Details", "Account", new { name = targetName });
            }

            using (var database = new ApplicationDbContext())
            {
                var author = database.Users.FirstOrDefault(u => u.Email == authorName);
                var target = database.Users.FirstOrDefault(u => u.Email == targetName);

                var comment = new Comment(content, author.Id, target.Id);

                database.Comments.Add(comment);

                author.WrittenComments.Add(comment);

                target.ProfileComments.Add(comment);

                database.SaveChanges();

                return RedirectToAction("Details", "Account", new { name = target.Email });
            }
        }

        //GET: Comment/Edit
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var comment = database.Comments.FirstOrDefault(c => c.Id == id);

                if (comment == null)
                {
                    return HttpNotFound();
                }

                if (!IsAuthorizedToEdit(comment))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                var model = new CommentEditViewModel();
                model.Content = comment.Content;
                model.TargetName = comment.Target.UserName;

                return View(model);
            }
        }

        //POST: Comment/Edit
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int? id, CommentEditViewModel model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new ApplicationDbContext())
            {
                var comment = database.Comments.Where(c => c.Id == id).Include(c => c.Target).First();
                if (comment == null)
                {
                    return HttpNotFound();
                }
                if (!IsAuthorizedToEdit(comment))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                comment.Content = model.Content;
                database.Entry(comment).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();

                return RedirectToAction("Details", "Account", new { name = comment.Target.UserName });
            }
        }

        //GET: Comment/Delete
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var comment = database.Comments.FirstOrDefault(c => c.Id == id);

                if (comment == null)
                {
                    return HttpNotFound();
                }

                if (!IsAuthorizedToEdit(comment))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                var model = new CommentEditViewModel();
                model.Content = comment.Content;
                model.TargetName = comment.Target.UserName;

                return View(model);
            }
        }

        //POST: Comment/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var comment = database.Comments.Where(c => c.Id == id).Include(c => c.Target).FirstOrDefault();

                if (comment == null)
                {
                    return HttpNotFound();
                }

                if (!IsAuthorizedToEdit(comment))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                string name = comment.Target.UserName;

                database.Comments.Remove(comment);
                database.SaveChanges();

                return RedirectToAction("Details", "Account", new { @name = name });
            }
        }

        private bool IsAuthorizedToEdit(Comment comment)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = comment.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }
    }
}