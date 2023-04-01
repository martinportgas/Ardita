using Ardita.Models;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ardita.Controllers
{
    public class Authentication : Controller
    {
        private readonly IUserService _service;

        public Authentication(IUserService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            return await Task.Run<ActionResult> (() =>{
                ClaimsPrincipal claims = HttpContext.User;
                if (claims.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home", new { area = "General" });
                else
                    return RedirectToAction("Login", "Authentication");
            });
        }
        public async Task<IActionResult> Login()
        {
            return await Task.Run<ActionResult>(() => {
                ClaimsPrincipal claims = HttpContext.User;

                if (claims.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home", new { area = "General" });

                return View();
            });
               
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Authentication");
        }
        public async Task<IActionResult> ValidateLogin(LoginModel model)
        {
            var data = _service.GetAll().Result.ToList().Where(
                x => x.Username == model.Username && 
                x.Password == Extensions.Global.Encode(model.Password)
               );

            //Validate
            if (data.Count() > 0)
            {
                //Add Claim Here
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, "Test Identity"),
                    new Claim("Name", "Test")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = model.KeepLoggedIn

                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authenticationProperties);

                return RedirectToAction("Index", "Home", new { area = "General" });
            }

            TempData["ValidateErrorMessage"] = "User Not Found";

            return RedirectToAction("Login", "Authentication");
        }
    }
}
