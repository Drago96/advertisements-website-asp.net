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
                    .ToList();

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
                return View(model);
            }
                
        }

        //POST: Advertisement/Create
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

                    var advertisement = new Advertisement(sellerId, model.Title, model.Description, model.Price);

                    this.SetImage(advertisement, model.ImageUpload);

                    database.Advertisements.Add(advertisement);
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
                var advertisement = database.Advertisements.Where(a => a.Id == id).Include(a => a.Seller).First();

                if(advertisement==null)
                {
                    return HttpNotFound();
                }

                return View(advertisement);
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

            if (ImageUpload != null && ImageUpload.ContentLength != 0 && validImageTypes.Contains(ImageUpload.ContentType))
            {
                DirectoryInfo dir = Directory.CreateDirectory("/Downloads/c#/ExamProject/Exam-Project/ExamProject/ExamProject/uploads/"+ advertisement.SellerId);
                var uploadDir = "/Downloads/c#/ExamProject/Exam-Project/ExamProject/ExamProject/uploads/"+ advertisement.SellerId;
                var imagePath = Path.Combine(uploadDir, ImageUpload.FileName);
                ImageUpload.SaveAs(imagePath);
                advertisement.ImageUrl = "/uploads/"+advertisement.SellerId+"/"+ImageUpload.FileName;
                
                
                
            }
        }
    }
}