using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SiteCore.Data;
using SiteCore.Models;

namespace SiteCore.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AccountHelper accountHelper;
        public ManageController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager,  AccountHelper accountHelper)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            this.accountHelper = accountHelper;
        }
      
        [HttpGet]
        public async Task<IActionResult> InfoPersAccount(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен."
                : message == ManageMessageId.PersInput ? "Ваши данные приняты"
                : "";

            var model = new IndexViewModel();
            if (User.Identity != null)
            {
                var USER = await userManager.FindByNameAsync(User.Identity.Name);
                model.FAM = USER.FAM;
                model.IM = USER.IM;
                model.OT = USER.OT;
                model.OT = USER.OT;
                model.PhoneNumber = USER.PhoneNumber;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InfoPersAccount(IndexViewModel m)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            if (User.Identity != null)
            {
                var USER = await AppUser();
                USER.FAM = m.FAM;
                USER.IM = m.IM;
                USER.OT = m.OT;
                USER.PhoneNumber = m.PhoneNumber;
                await userManager.UpdateAsync(USER);
            }
            return RedirectToAction(nameof(InfoPersAccount), new { Message = ManageMessageId.PersInput });
        }

        private async Task<ApplicationUser> AppUser()
        {
            return await userManager.FindByNameAsync(User.Identity.Name);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var USER = await AppUser();
            var result = await userManager.ChangePasswordAsync(USER, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var res = await accountHelper.UpdatePasswordClaimRole(USER, model.NewPassword);
                if (!res.Succeeded)
                {
                    AddErrors(result);
                }
                await signInManager.SignInAsync(USER, false);

               
                return RedirectToAction(nameof(InfoPersAccount), new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
        }

        public ActionResult Error()
        {
            var model = new ErrorViewModel();
            if (TempData.ContainsKey("Error"))
            {
                model.Errors.AddRange((List<string>)TempData["Error"]);
            }
            if (TempData.ContainsKey("ReturnURL"))
            {
                model.returnURL = (string)TempData["ReturnURL"];
            }
            return View(model);
        }
    }

    public enum ManageMessageId
    {
        ChangePasswordSuccess,
        PersInput
    }
}
