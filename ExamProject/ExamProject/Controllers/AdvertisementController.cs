using ExamProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ExamProject.Controllers
{
    public class AdvertisementController : Controller
    {
        // GET: Advertisement
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            using (var database = new ApplicationDbContext())
            {
                var advertisements = database.Advertisements
                    .Include(a => a.Seller)
                    .Include(a => a.Category)
                    .ToList();

                ViewBag.categories =new List<string>{ "All" };
                ViewBag.categories.AddRange(database.Categories.OrderBy(c => c.Name).Select(c => c.Name).ToList());

                return View(advertisements);

            }
        }

        //GET: Advertisement/Create
        [Authorize]
        public ActionResult Create()
        {
            using (var database = new ApplicationDbContext())
            {
                var model = new AdvertisementViewModel();
                model.Categories = database.Categories.OrderBy(c => c.Name).ToList();
                return View(model);
            }
                
        }

        //POST: Advertisement/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(AdvertisementViewModel model)
        {
            if(ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    var sellerId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    var advertisement = new Advertisement(sellerId, model.Title, model.Description, model.Price, model.CategoryId);

                    var seller = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First();

                    seller.Advertisements.Add(advertisement);



                    database.Entry(seller).State = EntityState.Modified;
                    database.Advertisements.Add(advertisement);
                    database.SaveChanges();

                    this.SetImage(advertisement, model.ImageUpload);


                    database.Entry(advertisement).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        //GET: Advertisement/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var advertisement = database.Advertisements.Where(a => a.Id == id).Include(a => a.Seller).Include(a => a.Category).First();

                if(advertisement==null)
                {
                    return HttpNotFound();
                }

                return View(advertisement);
            }
        }

        //GET: Advertisement/Delete
        public ActionResult Delete(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new ApplicationDbContext())
            {
                var advertisement = database.Advertisements.Where(a => a.Id == id).Include(a => a.Category).First();

                if(!IsAuthorizedToEdit(advertisement))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if(advertisement == null)
                {
                    return HttpNotFound();
                }

                return View(advertisement);
            }
        }

        //POST: Advertisement/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var advertisement = database.Advertisements.Where(a => a.Id == id).First();

                if(advertisement == null)
                {
                    return HttpNotFound();
                }

                if (!IsAuthorizedToEdit(advertisement))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                var fullPath = Server.MapPath("~") + advertisement.ImageUrl.Substring(1);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                var directory = new DirectoryInfo(Server.MapPath("~") + "uploads/" + advertisement.Id);
                if(directory.Exists)
                {
                    directory.Delete(true);
                }

                database.Advertisements.Remove(advertisement);
                database.SaveChanges();

                return RedirectToAction("List");
            }
        }

        //GET: Advertisement/Edit
        public ActionResult Edit (int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var advertisement = database.Advertisements.Where(a => a.Id == id).First();

                if(advertisement == null)
                {
                    return HttpNotFound();
                }

                if (!IsAuthorizedToEdit(advertisement))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                var model = new AdvertisementEditViewModel();
                model.Description = advertisement.Description;
                model.Price = advertisement.Price;
                model.Title = advertisement.Title;
                model.ImageUrl = advertisement.ImageUrl;
                model.IsSold = advertisement.IsSold;
                model.CategoryId = advertisement.CategoryId;
                model.Categories = database.Categories.OrderBy(c => c.Name).ToList();
                ViewBag.Id = advertisement.Id;

                return View(model);
            }
        }

        //POST: Advertisement/Edit
        [HttpPost]
        public ActionResult Edit(int? id, AdvertisementEditViewModel model)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new ApplicationDbContext())
            {
                var advertisement = database.Advertisements.Where(a => a.Id == id).First();

                if(advertisement == null)
                {
                    return HttpNotFound();
                }

                if (!IsAuthorizedToEdit(advertisement))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                advertisement.Title = model.Title;
                advertisement.Description = model.Description;
                advertisement.Price = model.Price;
                advertisement.IsSold = model.IsSold;
                advertisement.CategoryId = model.CategoryId;
                if(model.ImageUpload!=null)
                {
                    this.SetImage(advertisement, model.ImageUpload);
                }
                
                database.Entry(advertisement).State = EntityState.Modified;
                database.SaveChanges();

                return RedirectToAction("Details", "Advertisement", new { @id = advertisement.Id });
            }


        }

        //POST: Mark as sold
        public ActionResult MarkAsSold(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var advertisement = database.Advertisements.Where(a => a.Id == id).First();

                if (!IsAuthorizedToEdit(advertisement))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                advertisement.MarkSold();

                database.Entry(advertisement).State = EntityState.Modified;
                database.SaveChanges();

                return RedirectToAction("Details", "Advertisement", new { @id = advertisement.Id });

            }
        }

        //GET: Advertisement/Search
        public ActionResult Search (string categoryName,string searchString)
        {
            if(searchString == null || (searchString=="" && categoryName == "All") || categoryName == "" || categoryName == null)
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

        private void SetImage(Advertisement advertisement, HttpPostedFileBase ImageUpload)
        {
            var validImageTypes = new string[]
             {
                 "image/gif",
                 "image/jpeg",
                 "image/pjpeg",
                 "image/png"
             };

            if(advertisement.ImageUrl!=null)
            {
                var fullPath = Server.MapPath("~") + advertisement.ImageUrl.Substring(1);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            if (ImageUpload != null && ImageUpload.ContentLength != 0 && validImageTypes.Contains(ImageUpload.ContentType))
            {
                DirectoryInfo dir = Directory.CreateDirectory(Server.MapPath("~/uploads/")+ advertisement.Id);
                var uploadDir = Server.MapPath("~/uploads/") + advertisement.Id;
                var imagePath = Path.Combine(uploadDir, ImageUpload.FileName);
                ImageUpload.SaveAs(imagePath);
                advertisement.ImageUrl = "/uploads/"+advertisement.Id + "/"+ImageUpload.FileName;
                
                
                
            }
            
        }

        private bool IsAuthorizedToEdit(Advertisement advertisement)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isSeller = advertisement.IsSeller(this.User.Identity.Name);

            return isAdmin || isSeller;
        }
    }
}