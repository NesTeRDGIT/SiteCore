using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLoaderMedpomData;
using SiteCore.Data;
using SiteCore.Models;

namespace SiteCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly AccountHelper accountHelper;
        private readonly ILogger logger;
        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, RoleManager<ApplicationRole> _roleManager, AccountHelper accountHelper, ILogger logger)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            this.accountHelper = accountHelper;
            this.logger = logger;
        }
      
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel{ReturnUrl = returnUrl });
        }

    
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
               var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> RegisterUser(string IDUser, string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.Roles = roleManager.Roles.OrderBy(x => x.Id);
            return View(await createRegisterNewViewModel(IDUser));
        }

        private async Task<RegisterNewViewModel> createRegisterNewViewModel(string IDUser)
        {
            if (string.IsNullOrEmpty(IDUser))
                return new RegisterNewViewModel();

            var USER = await userManager.FindByIdAsync(IDUser);

            if (USER == null)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return new RegisterNewViewModel();
            }

            return new RegisterNewViewModel
            {
                UserName = USER.UserName,
                CODE_MO = USER.CODE_MO,
                CODE_SMO = USER.CODE_SMO,
                Roles = USER.UserRoles?.Select(x => x.RoleId).ToArray()?? new string[0],
                UserId = USER.Id,
                WithSing = USER.WithSing
            };
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterNewViewModel model, string ReturnUrl)
        {
            ApplicationUser user = null;
            var isNEW = string.IsNullOrEmpty(model.UserId);
            var isChangePassword = isNEW || !string.IsNullOrEmpty(model.Password);
            try
            {
                if (ModelState.IsValid)
                {
                    if (isNEW)
                    {
                        user = new ApplicationUser { UserName = model.UserName, CODE_MO = model.CODE_MO, WithSing = model.WithSing, CODE_SMO = model.CODE_SMO, UserRoles = new List<ApplicationUserRole>() };
                        var result = await userManager.CreateAsync(user, model.Password);
                        if (!result.Succeeded)
                        {
                            AddErrors(result);
                        }
                    }
                    else
                    {
                        user = await userManager.FindByIdAsync(model.UserId);
                        if (user == null)
                            ModelState.AddModelError("", $"Пациент с номером {model.UserId} не найден");
                        else
                        {
                            user.UserName = model.UserName;
                            user.CODE_MO = model.CODE_MO;
                            user.WithSing = model.WithSing;
                            user.CODE_SMO = model.CODE_SMO;
                            var result = await userManager.UpdateAsync(user);
                            if (!result.Succeeded)
                            {
                                AddErrors(result);
                            }

                            if (isChangePassword)
                            {
                                result = await userManager.ChangePasswordAsync(user, user.PasswordClaim().ClaimValue, model.Password);
                                if (!result.Succeeded)
                                {
                                    AddErrors(result);
                                }
                            }
                            
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    if (isChangePassword)
                    {
                        var res = await accountHelper.UpdatePasswordClaimRole(user, model.Password);
                        if (!res.Succeeded)
                        {
                            AddErrors(res);
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var res = await accountHelper.UpdateRole(user, model.Roles);
                    if (!res.resultRemove.Succeeded)
                    {
                        AddErrors(res.resultRemove);
                    }

                    if (!res.resultAdd.Succeeded)
                    {
                        AddErrors(res.resultRemove);
                    }
                }

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }

                    return RedirectToAction(nameof(RegisterUsers));
                }

                if (isNEW && user!=null)
                    await userManager.DeleteAsync(user);
                ViewBag.ReturnUrl = ReturnUrl;
                ViewBag.Roles = roleManager.Roles.OrderBy(x => x.Id);
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                if (isNEW)
                    await userManager.DeleteAsync(user);
                ViewBag.ReturnUrl = ReturnUrl;
                ViewBag.Roles = roleManager.Roles.OrderBy(x => x.Id);
                return View(model);

            }
            
        }
      
        private void AddErrors(IdentityResult result)
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult RegisterUsers()
        {
            if(TempData.ContainsKey("Message"))
                ViewBag.Message = TempData["Message"].ToString();
            return View(createRegisterUsersViewModel());
        }

        private RegisterUsersViewModel createRegisterUsersViewModel()
        {
            var model = new RegisterUsersViewModel();
            foreach (var us in userManager.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).Include(x => x.CODE_MO_NAME).Include(x => x.Claims).OrderBy(x => x.CODE_MO))
            {
                model.Users.Add(new RegisterUser
                {
                    CODE_MO = us.CODE_MO,
                    Id = us.Id,
                    NAME_MO = us.CODE_MO_NAME?.NAM_MOK,
                    Online = false,
                    Roles = string.Join(",", us.UserRoles.Select(x => x.Role.Name)),
                    UserName = us.UserName,
                    NUMBER = us.PhoneNumber,
                    FIO = us.FIO,
                    Password = us.PasswordClaim().ClaimValue
                });
            }

            return model;
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string IDUser, string ReturnUrl)
        {
            var user = await userManager.FindByIdAsync(IDUser);
            if (user != null)
            {
                var res= await userManager.DeleteAsync(user);
                if(res.Succeeded)
                    ViewBag.Message = $"Пользователь {user.UserName} успешно удалён";
                else
                {
                    ViewBag.Message = $"Ошибка удаления пользователя {user.UserName}";
                    AddErrors(res);
                }
            }
            else
            {
                ModelState.AddModelError("", $"Пользователь №{IDUser} не найден");
            }

            if (ViewBag.Message != null)
            {
                TempData.Add("Message", ViewBag.Message);
            }
            return RedirectToAction(nameof(RegisterUsers));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult MassRegister()
        {
            var model = new MassRegisterViewModel { FirstValue = 750001, LastValue = 750003, UserNamePrefix = "test" };
            ViewBag.Roles = roleManager.Roles.OrderBy(x => x.Id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MassRegister(MassRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                for (var i = model.FirstValue; i <= model.LastValue; i++)
                {
                    var user_name = $"{model.UserNamePrefix}{i}";
                    var pass = accountHelper.GetRandomPassword();
                    var code_mo = i.ToString();
                    var user = new ApplicationUser { UserName = user_name, CODE_MO = code_mo, UserRoles = new List<ApplicationUserRole>() };
                    var result = await userManager.CreateAsync(user, pass);
                 
                    if (result.Succeeded)
                    {
                        user.PasswordClaim().ClaimValue = pass;
                        var resultUpdate = await userManager.UpdateAsync(user);
                        if (!resultUpdate.Succeeded)
                        {
                            AddErrors(resultUpdate);
                        }
                        var res = await accountHelper.UpdateRole(user, model.Roles);
                        if (!res.resultRemove.Succeeded)
                        {
                            AddErrors(res.resultRemove);
                        }
                        if (!res.resultAdd.Succeeded)
                        {
                            AddErrors(res.resultRemove);
                        }
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            return RedirectToAction(nameof(RegisterUsers));
        }

       


    }

    public class UpdateRoleResult
    {
        public UpdateRoleResult(IdentityResult resultRemove, IdentityResult resultAdd)
        {
            this.resultRemove = resultRemove;
            this.resultAdd = resultAdd;
        }
        public IdentityResult resultRemove { get; set; }
        public IdentityResult resultAdd { get; set; }
    }
    public  class AccountHelper
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public AccountHelper(UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<UpdateRoleResult> UpdateRole(ApplicationUser user, IList<string> Roles)
        {
            var RoleAll = roleManager.Roles.ToList();
            var NewRoleUser = (from RoleId in Roles select RoleAll.FirstOrDefault(x => x.Id == RoleId) into r where r != null select r.Name).ToList();
            var CurrentRoleUser = user.UserRoles.Select(x => x.Role.Name).ToList();
            var removeRole = CurrentRoleUser.Where(x => !NewRoleUser.Contains(x)).ToList();
            var newRole = NewRoleUser.Where(x => !CurrentRoleUser.Contains(x)).ToList();
            var resultRemove = await userManager.RemoveFromRolesAsync(user, removeRole);
            var resultAdd = await userManager.AddToRolesAsync(user, newRole);

            return new UpdateRoleResult(resultRemove, resultAdd);
        }

        public Task<IdentityResult> UpdatePasswordClaimRole(ApplicationUser user, string Password)
        {
            user.PasswordClaim().ClaimValue = Password;
            return userManager.UpdateAsync(user);
        }


        public string GetRandomPassword()
        {
            var rslt = "";
            var rnd = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i <= 5; i++)
            {
                if (i % 2 == 0)
                    rslt += Convert.ToChar(rnd.Next(65, 90));
                else
                    rslt += Convert.ToChar(rnd.Next(48, 57));
            }
            return rslt;
        }
    }

}
