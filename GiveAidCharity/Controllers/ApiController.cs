﻿using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    public class ApiController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApiController()
        {
        }

        public ApiController(
            ApplicationUserManager userManager
        )
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }



        // GET: Api
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> AjaxCheckUserName(string username)
        {
            var validationResult = await UserManager.FindByNameAsync(username) == null;
            Debug.WriteLine(validationResult);
            return Json(validationResult);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> AjaxCheckEmail(string email)
        {
            var validationResult = await UserManager.FindByEmailAsync(email) == null;
            Debug.WriteLine(validationResult);
            return Json(validationResult);
        }
    }
}