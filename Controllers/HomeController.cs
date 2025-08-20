using KutuphaneMvc.Models.Entity;
using System;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace KutuphaneMvc.Controllers
{
    public class HomeController : Controller
    {
        private LibraryDBEntities1 db = new LibraryDBEntities1();

        public ActionResult Index()
        {
            ViewBag.TotalBooks = db.KITAP.Count();
            ViewBag.TotalAvailableBooks = db.KITAP.Count(x => x.DURUM == true);
            ViewBag.ActiveLoans = db.EMANET.Count();
            ViewBag.Overdue = db.EMANET.Count(x => x.TESLIM_TARIHI < DateTime.Now && x.TESLIM_EDILDI_MI == false);

            // 1. Adım: EF'den veriyi çekerken sade listede tut
            var rawData = db.KITAP
                .Join(db.KATEGORI,
                      kitap => kitap.KATEGORI_ID,
                      kategori => kategori.KATEGORI_ID,
                      (kitap, kategori) => kategori.KATEGORI_ADI)
                .ToList(); // SQL burada biter

            // 2. Adım: ToList sonrası artık LINQ to Objects - burada dynamic kullanabiliriz
            var kategoriDagilim = rawData
                .GroupBy(k => k)
                .Select(g =>
                {
                    dynamic obj = new ExpandoObject();
                    obj.Kategori = g.Key;
                    obj.KitapSayisi = g.Count();
                    return obj;
                })
                .ToList();

            ViewBag.CategoryStats = kategoriDagilim;

            // Raf doluluk
            var raflar = db.RAF.ToList();
            var rafDurum = raflar.Select(r =>
            {
                dynamic obj = new ExpandoObject();
                obj.ShelfName = r.RAF_AD;
                obj.Used = db.KITAP.Count(k => k.RAF_ID == r.RAF_ID);
                obj.Capacity = r.KAPASITE;
                return obj;
            }).ToList();

            ViewBag.ShelfStatus = rafDurum;

            return View();
        }
    }
}