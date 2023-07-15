﻿using Ardita.Extensions;
using Ardita.Models;
using Ardita.Models.DbModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ardita.Controllers
{
    public class Authentication : Controller
    {
        private readonly IUserService _userService;


        public Authentication(
            IUserService userService
            )
        {
            _userService = userService;
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
            var username = model.Username;
            var password = Extensions.Global.Encode(model.Password);
            var claims = await _userService.GetLogin(username, password);

            //Validate
            if (claims != null)
            {
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
        public async Task<IActionResult> ChangePassword() 
        {
            ViewBag.UserId = AppUsers.CurrentUser(User).UserId;
            ViewBag.Username = AppUsers.CurrentUser(User).Username;
            return View();
        }
        public async Task<IActionResult> Save(MstUser model)
        {

            if (model is not null)
            {
                model.PasswordLast = Global.Encode(model.PasswordLast);
                model.Password = Global.Encode(Request.Form["txtPasswordNew"].ToString());
                await _userService.ChangePassword(model);
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Authentication");
        }
        [HttpGet]
        public async Task<JsonResult> FindPassword(Guid Id, string Password)
        {

            if (Id != Guid.Empty && !string.IsNullOrEmpty(Password))
            {
                var data = await _userService.FindPasswordByUsername(Id, Global.Encode(Password));
                if (data)
                    return Json(true);
                else
                    return Json(false);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
