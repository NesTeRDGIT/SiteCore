using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SiteCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiteCore.Class;
using SiteCore.Data;
using ServiceLoaderMedpomData;
using ILogger = ServiceLoaderMedpomData.ILogger;

namespace SiteCore.Controllers
{
    public class HomeController : Controller
    {
        private MyOracleSet db { get; }
        private  RoleManager<ApplicationRole> roleManager { get; }
        private ILogger logger;
        public HomeController(MyOracleSet db, RoleManager<ApplicationRole> roleManager, ILogger logger)
        {
            this.db = db;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetNewsList());
        }
        public IActionResult About()
        {
            return View();
        }
        private async Task<List<NEWS>> GetNewsList()
        {
            var news = new List<NEWS>();
            if (User.Identity?.IsAuthenticated==true)
            {
                var Roles = User.Claims.Where(x => x.Type == "ROLE_ID").Select(x => x.Value).ToList();
                news = await db.NEWS.Where(x => x.NEWS_ROLE.Count(y => Roles.Contains(y.ID_ROLE)) != 0).OrderByDescending(x => x.DT).ToListAsync();
            }
            return news;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditNews(int? NEWS_ID = null)
        {
            var item = NEWS_ID.HasValue ? await db.NEWS.Include(x=>x.NEWS_ROLE).FirstOrDefaultAsync(x => x.ID_NEW == NEWS_ID.Value) : new NEWS {TEXT_STR = ""};
            var model = new NewsEditModelView
            {
                DT = item.DT,
                HEADER = item.HEADER,
                ID_NEW = NEWS_ID,
                TEXT = item.TEXT_STR,
                NEWS_ROLE = item.NEWS_ROLE.Select(x=>x.ID_ROLE).ToArray(),
                Roles = await roleManager.Roles.OrderBy(x => x.Id).ToArrayAsync()
            };
            return PartialView(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<CustomJsonResult> EditNews(NewsEditModelView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NEWS news = null;
                    if (model.ID_NEW.HasValue)
                    {
                        news = await db.NEWS.Include(x=>x.NEWS_ROLE).FirstOrDefaultAsync(x => x.ID_NEW == model.ID_NEW.Value);
                    }
                    else
                    {
                        news = new NEWS();
                        db.NEWS.Add(news);
                    }

                    if (news == null)
                        throw new ModelException("", "Не удалось найти новость для редактирования");
                    news.TEXT_STR = model.TEXT;
                    news.HEADER = model.HEADER;
                    news.NEWS_ROLE.Clear();
                    foreach (var rol in model.NEWS_ROLE)
                    {
                        news.NEWS_ROLE.Add(new NEWS_ROLE {ID_ROLE = rol});
                    }
                    await db.SaveChangesAsync();
                    return CustomJsonResult.Create(null);
                }

                throw new ModelException(null, "");
            }
            catch (ModelException ex)
            {
                if(ex.Key!=null)
                    ModelState.AddModelError("", ex.Message);
                model.Roles = await roleManager.Roles.OrderBy(x => x.Id).ToArrayAsync();
                return CustomJsonResult.Create(await this.RenderViewAsync("EditNews", model, true),false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(EditNews)}: {ex.Message}", LogType.Error);
                ModelState.AddModelError("","Внутренняя ошибка сервиса");
                model.Roles = await roleManager.Roles.OrderBy(x => x.Id).ToArrayAsync();
                return CustomJsonResult.Create(await this.RenderViewAsync("EditNews", model, true),false);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<CustomJsonResult> DeleteNews(int ID_NEW)
        {
            try
            {
                var news = await db.NEWS.FirstOrDefaultAsync(x => x.ID_NEW == ID_NEW);
                if (news != null)
                {
                    db.NEWS.Remove(news);
                    await db.SaveChangesAsync();
                    return CustomJsonResult.Create(null);
                }

                throw new ModelException("", $"Новость с №{ID_NEW} не найдена");
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(DeleteNews)}: {ex.Message}", LogType.Error);
                return CustomJsonResult.Create("Внутренняя ошибка сервиса", false);
            }
        }

        

    }
}
