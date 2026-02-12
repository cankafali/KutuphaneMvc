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

        /// <summary>
        /// Raflar ve ilişkili kitapların listelendiği görünümü döndürür.
        /// Veriler, raf ve kitap ilişkilerini sağlayan veritabanı görünümünden alınır.
        /// </summary>
        /// <returns>Raf ve kitap bilgilerini içeren liste.</returns>
        public ActionResult Index()
        {
            var data = db.View_RAF_KITAP.ToList(); // Raf ve kitap ilişkilerini sağlayan veritabanı görünümünü kullan
            return View(data);
        }

        public ActionResult Yeniuye()
        {
            return View();
        }
       
    }
}