using ImageTry.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageTry.Controllers
{
    public class HomeController : Controller
    {
        TryImageDBEntities db = new TryImageDBEntities();
        public ActionResult Index()
        {
            var data = db.students.ToList();
            return View(data);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(student std)
        {
            string fileName = Path.GetFileNameWithoutExtension(std.ImageFile.FileName);
            string extension = Path.GetExtension(std.ImageFile.FileName);
            HttpPostedFileBase postedFile = std.ImageFile;
            string FileLength = Convert.ToString(postedFile.ContentLength);

            if(extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
            {
                if (Convert.ToUInt64(FileLength) <= 1000000)
                {
                    fileName += extension;
                    std.Imagepath = "~/images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/images/"), fileName);

                    std.ImageFile.SaveAs(fileName);
                    db.students.Add(std);
                    int check = db.SaveChanges();
                    if (check > 0)
                    {
                        ViewBag.Message = "<script>alert('Successfully Recorded')</script>";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Message = "<script>alert('Failed!!')</script>";
                    }

                }
                else
                {
                    ViewBag.SizeMessage = "<script>alert('Size Should be less than or equal to 1 MB!!')</script>";
                }
            }
            else
            {
                ViewBag.ExtensionMessage = "<script>alert('Image Not Supported!!')</script>";
            }
            
            return View();
        }

       
    }
}