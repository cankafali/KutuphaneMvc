using KutuphaneMvc.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KutuphaneMvc.Controllers
{
    public class UyeController : Controller
    {
        private LibraryDBEntities1 db = new LibraryDBEntities1();

        public ActionResult Index()
        {
            var uye = db.UYE.ToList();
            return View(uye);
        }

        [HttpGet]
        public ActionResult YeniUye()
        {
            ViewBag.Yetkiler = db.ROLE
                .Select(r => new SelectListItem
                {
                    Value = r.ROLE_ID.ToString(),
                    Text = r.ROLE_AD
                }).ToList();

            return View(new UYE { UYELIK_TARIHI = DateTime.Now, CEZA_PUAN = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YeniUye(UYE m)
        {
            // ModelState hatalıysa sayfaya döneceğiz -> listeyi TEKRAR doldur!
            ViewBag.Yetkiler = db.ROLE
                .Select(r => new SelectListItem
                {
                    Value = r.ROLE_ID.ToString(),
                    Text = r.ROLE_AD
                }).ToList();

            if (!ModelState.IsValid)
                return View(m);

            if (!m.UYELIK_TARIHI.HasValue) m.UYELIK_TARIHI = DateTime.Now;
            if (!m.CEZA_PUAN.HasValue) m.CEZA_PUAN = 0;

            db.UYE.Add(m);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Aski()
        {
            var askidakiUyeler = db.UYE.Where(u => u.CEZA_PUAN >= 50).ToList();
            return View(askidakiUyeler);
        }

        private bool UyeAskidaMi(UYE uye)
        {
            return uye.CEZA_PUAN >= 50;
        }

        private bool UyeBanliMi(UYE uye)
        {
            return uye.CEZA_PUAN >= 100;
        }

        // Üye işlemlerinde kullanmak için (örneğin ödünç alma, rezervasyon vs.)
        private bool UyeIslemYapabilirMi(UYE uye)
        {
            return uye.CEZA_PUAN < 50; // 50'den az ceza puanı olanlar işlem yapabilir
        }
    }
}