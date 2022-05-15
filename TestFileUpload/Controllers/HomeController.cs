using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFileUpload.Models;

namespace TestFileUpload.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        public ActionResult Index()
        {
            dbFilesEntities db = new dbFilesEntities();
            return View(db.Files.ToList());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            File file = new File();
            string filename = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
            string extension = System.IO.Path.GetExtension(postedFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            file.Name = filename;
            file.Path = "~/Uploads/" + filename;
            //filename = System.IO.Path.Combine(Server.MapPath("~/Uploads"), filename);
            //postedFile.SaveAs(filename);

            using (dbFilesEntities db = new dbFilesEntities())
            {
                db.Files.Add(file);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public ActionResult Download(int id)
        {
            dbFilesEntities db = new dbFilesEntities();
            File file = db.Files.Where(x => x.FileId == id).FirstOrDefault();
            if (System.IO.File.Exists(Server.MapPath(file.Path)))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(file.Path));
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
            }

            return RedirectToAction("Index");
        }
    }
}