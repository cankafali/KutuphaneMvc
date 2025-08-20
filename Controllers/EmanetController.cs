using KutuphaneMvc.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace KutuphaneMvc.Controllers
{
    public class EmanetController : Controller
    {
        private LibraryDBEntities1 db = new LibraryDBEntities1();

        public ActionResult Index()
        {
            List<View_EmanetKitap> emanetList = db.View_EmanetKitap.ToList();

            return View(emanetList);
        }

        public ActionResult Emanet(int? kitapId)
        {
            ViewBag.BookList = db.KITAP.Where(x => x.DURUM == true).ToList();
            ViewBag.UserList = (from item in db.UYE
                                select new
                                {
                                    UserID = item.UYE_ID,
                                    AdSoyad = item.AD + " " + item.SOYAD,
                                });

            var model = new EMANET
            {
                KITAP_ID = kitapId ?? 0,
                ALINIS_TARIHI = DateTime.Today,
                TESLIM_TARIHI = DateTime.Today.AddDays(15),
                TESLIM_EDILDI_MI = false   // default
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EmanetKaydet(int KITAP_ID, int UYE_ID, DateTime ALINIS_TARIHI, DateTime TESLIM_TARIHI)
        {
            var kitap = db.KITAP.FirstOrDefault(k => k.KITAP_ID == KITAP_ID);
            if (kitap == null)
            {
                return HttpNotFound();
            }
            var e = new EMANET
            {
                KITAP_ID = KITAP_ID,
                UYE_ID = UYE_ID,
                ALINIS_TARIHI = ALINIS_TARIHI,
                TESLIM_TARIHI = TESLIM_TARIHI,
                TESLIM_EDILDI_MI = false
            };
            var uye = db.UYE.FirstOrDefault(u => u.UYE_ID == e.UYE_ID);
            if (uye != null && uye.CEZA_PUAN >= 100)
            {
                ViewBag.BookList = db.KITAP.Where(x => x.DURUM == true).ToList();
                ViewBag.UserList = (from item in db.UYE
                                    select new
                                    {
                                        UserID = item.UYE_ID,
                                        AdSoyad = item.AD + " " + item.SOYAD,
                                    });
                ModelState.AddModelError("", "Bu üye banlı olduğu için ödünç alamaz!!!!");
                return View("Emanet", e);
            }
            db.EMANET.Add(e);

            if (kitap != null)
            {
                kitap.DURUM = false;
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult EmanetSil(int id)
        {
            var b = db.EMANET.Find(id);
            if (b == null)
                return HttpNotFound();
            db.EMANET.Remove(b);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}