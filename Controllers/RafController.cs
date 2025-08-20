using KutuphaneMvc.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KutuphaneMvc.Controllers
{
    public class RafController : Controller
    {
        private LibraryDBEntities1 db = new LibraryDBEntities1();

        public ActionResult Index()
        {
            var data = db.View_RAF_KITAP.ToList(); // View'i kullanıyorsun zaten
            return View(data);
        }

        public ActionResult Yeniuye()
        {
            return View();
        }
       
    }
}