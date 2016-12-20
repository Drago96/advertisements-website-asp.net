using ExamProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //GET: Home/List
        public ActionResult List()
        {
            using (var database = new ApplicationDbContext())
            {
                var advertisements = database.Advertisements
                    .Include(a => a.Seller)
                    .Include(a => a.Category)
                    .ToList();

                ViewBag.categories = new List<string> { "All" };
                ViewBag.categories.AddRange(database.Categories.OrderBy(c => c.Name).Select(c => c.Name).ToList());

                return View(advertisements);

            }
        }

        //GET: Home/Search
        public ActionResult Search(string categoryName, string searchString)
        {
            if (searchString == null || (searchString == "" && categoryName == "All") || categoryName == "" || categoryName == null)
            {
                return RedirectToAction("List");
            }
            using (var database = new ApplicationDbContext())
            {
                var searchWords = searchString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var advertisements = new List<Advertisement>();
                if (categoryName == "All")
                {
                    foreach (var searchWord in searchWords)
                    {
                        advertisements.AddRange(database.Advertisements
                            .Where(a => a.Title.Contains(searchWord))
                            .Include(a => a.Seller)
                            .Include(a => a.Category)
                            .ToList());
                    }
                }
                else
                {
                    var category = database.Categories.Where(c => c.Name == categoryName).FirstOrDefault();

                    if (searchWords.Count() == 0)
                    {
                        advertisements.AddRange(database.Advertisements
                         .Where(a => a.CategoryId == category.Id)
                         .Include(a => a.Seller)
                         .Include(a => a.Category)
                         .ToList());
                    }
                    else
                    {
                        foreach (var searchWord in searchWords)
                        {
                            advertisements.AddRange(database.Advertisements
                               .Where(a => a.Title.Contains(searchWord) && a.CategoryId == category.Id)
                               .Include(a => a.Seller)
                               .Include(a => a.Category)
                               .ToList());
                        }
                    }


                }
                ViewBag.searchString = searchString;
                ViewBag.categoryName = categoryName;
                ViewBag.categories = new List<string> { "All" };
                ViewBag.categories.AddRange(database.Categories.OrderBy(c => c.Name).Select(c => c.Name).ToList());
                return View(advertisements);
            }
        }


    }

    
}