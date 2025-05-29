using System.Web.Mvc;
using System.Web.Security;
using TrabalhoFinal3.Models;

public class AccountController : Controller
{
    // GET: /Account/Login
    public ActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(LoginViewModel model, string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Aqui deve validar o utilizador com base na BD
        // Exemplo simples (substituir pela sua lógica real):
        if (model.Email == "admin@exemplo.com" && model.Password == "1234")
        {
            FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        ModelState.AddModelError("", "Login inválido.");
        return View(model);
    }

    // GET: /Account/Logout
    public ActionResult Logout()
    {
        FormsAuthentication.SignOut();
        return RedirectToAction("Login", "Account");
    }

    public ActionResult Test()
    {
        return Content("Controller funciona!");
    }
}
