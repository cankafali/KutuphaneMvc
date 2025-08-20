using System.Web.Mvc;

namespace KutuphaneMvc.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            // Simple demo authentication. In a real app, replace with database check.
            if (username == "admin" && password == "admin")
            {
                Session["Username"] = username;
                Session["Role"] = "Admin";
                return RedirectToAction("Index", "Home");
            }
            if (username == "user" && password == "user")
            {
                Session["Username"] = username;
                Session["Role"] = "User";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
