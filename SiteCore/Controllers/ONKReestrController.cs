using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SiteCore.Class;
using SiteCore.Data;
using SiteCore.Models;

namespace SiteCore.Controllers
{
    [Authorize(Roles = "ONKReestr")]
    public class ONKReestrController : Controller
    {
        private ONKOracleSet db { get; }
        private UserInfoHelper userInfoHelper;
        private UserInfo _userInfo;
        private UserInfo userInfo
        {
            get
            {
                if (_userInfo == null)
                {
                    _userInfo = userInfoHelper.GetInfo(User.Identity.Name);
                }
                return _userInfo;
            }
        }

        public ONKReestrController(ONKOracleSet _db, UserInfoHelper userInfoHelper)
        {
            db = _db;
            this.userInfoHelper = userInfoHelper;
        }
        public IActionResult ONKReestr()
        {
            return View();
        }

        public CustomJsonResult GetONKList(int Page = 1, int CountOnPage = 50)
        {
            var nodes = GetONKReestr();
          
            var model = new GetONKListModel()
            {
                Items = nodes.Skip((Page - 1) * CountOnPage).Take(CountOnPage).ToList(),
                Pages = new PageAreaModel()
                {
                    CountOnPage = CountOnPage,
                    CurrentPage = Page,
                    PageCount = Math.Ceiling(Convert.ToDecimal(nodes.Count()) / Convert.ToDecimal(CountOnPage))
                }
            };
            return CustomJsonResult.Create(model);
        }

     

        private IQueryable<ONKReestr> GetONKReestr(int? filter = null)
        {
            var nodes = userInfo.CODE_SMO != "75" ? db.ONKReestr.Where(x => !string.IsNullOrEmpty(x.SMO) && x.SMO == userInfo.CODE_SMO) : db.ONKReestr;
            return nodes.OrderByDescending(x => x.ONK_REESTR_ID);
        }

        [HttpGet]
        public CustomJsonResult SLList(int ONK_REESTR_ID)
        {
            return CustomJsonResult.Create(GetONKReestrSL(ONK_REESTR_ID).Select(x=>new
            {
                x.ID,
                x.MONTH,
                x.YEAR,
                x.SLUCH_Z_ID,
                x.ONK_REESTR_ID,
                x.V_SL_MINI.LPU_NAME,
                x.V_SL_MINI.N_USL_OK,
                x.V_SL_MINI.DATE_1,
                x.V_SL_MINI.DATE_2,
                x.V_SL_MINI.N_DS1,
                x.V_SL_MINI.N_RSLT,
                x.V_SL_MINI.DS_ONK,
                x.V_SL_MINI.N_SMO,
                x.V_SL_MINI.NSCHET,
                x.V_SL_MINI.DSCHET

            }).ToList());
        }
        private ICollection<ONK_REESTR_SL> GetONKReestrSL(int ONK_REESTR_ID)
        {
            var nodes = db.ONKReestr
                .Include(x => x.SL)
                .ThenInclude(x => x.V_SL_MINI)
                .FirstOrDefault(x => (!string.IsNullOrEmpty(x.SMO) && x.SMO == userInfo.CODE_SMO || userInfo.CODE_SMO == "75") && x.ONK_REESTR_ID == ONK_REESTR_ID);
                
              
            return nodes?.SL ?? new List<ONK_REESTR_SL>();
        }

        public IActionResult ViewZ_SL(int SLUCH_Z_ID)
        {
            var zSl = db.FindZ_SL(SLUCH_Z_ID);
            return View(zSl);
        }

       
    }
}
