using ExamProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ExamProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

       
        //GET: Category/List
        public ActionResult List()
        {
            using (var database = new ApplicationDbContext())
            {
                var categories = database.Categories.ToList();

                return View(categories);
            }
        }

        //GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    database.Categories.Add(category);
                    database.SaveChanges();
                }

                return RedirectToAction("List");
            }

            return View(category);
        }

        //GET: Category/Edit
        public ActionResult Edit(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var category = database.Categories.FirstOrDefault(c => c.Id == id);

                if(category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }
        }
        
        //POST: Category/Edit
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    database.Entry(category).State = System.Data.Entity.EntityState.Modified;
                    database.SaveChanges();
                }

                return RedirectToAction("List");
            }

            return View(category);
        }

        //GET: Category/Delete
        public ActionResult Delete(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var category = database.Categories.FirstOrDefault(c => c.Id == id);

                if(category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }
                
        }

        //POST: Category/Delete
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
                var category = database.Categories.FirstOrDefault(c => c.Id == id);

                var categoryAdvertisements = category.Advertisements.ToList();

                foreach(var advertisement in categoryAdvertisements)
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

                database.Categories.Remove(category);

                database.SaveChanges();

                return RedirectToAction("List");
            }
        }
    }
}