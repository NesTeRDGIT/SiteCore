using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelManager;
using Microsoft.AspNetCore.Authorization;
using SiteCore.Class;
using SiteCore.Data;

namespace SiteCore.Controllers
{
    [Authorize(Roles = "OZPZ, Admin")]
    public class OZPZController : Controller
    {
        private MyOracleSet myOracleSet;
        private IZPZExcelCreator zpzExcelCreator;
        public OZPZController(MyOracleSet myOracleSet, IZPZExcelCreator zpzExcelCreator)
        {
            this.myOracleSet = myOracleSet;
            this.zpzExcelCreator = zpzExcelCreator;
        }
        public IActionResult ZPZ_EFFECTIVENESS()
        {
            return View(new List<ZPZ_EFFECTIVENESS>());
        }
        [HttpGet]
        public IActionResult GET_EFFECTIVENESS_VIEW(DateTime dt1, DateTime dt2)
        {
            var val = new DataGet_ZPZ_EFFECTIVENESS
            {
                dt1 = dt1,
                dt2 = dt2,
                data = myOracleSet.Get_ZPZ_EFFECTIVENESS(dt1, dt2)
            };
            TempData["GET_EFFECTIVENESS_VIEW"] = val.ObjectToXml();
            return PartialView("_ZPZ_EFFECTIVENESSPartical", val.data);
        }
        [HttpGet]
        public IActionResult GET_EFFECTIVENESS_VIEWXLS()
        {
            var data = TempData["GET_EFFECTIVENESS_VIEW"].ToString().XmlToObject<DataGet_ZPZ_EFFECTIVENESS>();
            TempData["GET_EFFECTIVENESS_VIEW"] = data.ObjectToXml();
            return data == null ? null : File(zpzExcelCreator.CreateXLSEFFECTIVENESS(data.data), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Результативность с {data.dt1:yyyy-MM} по {data.dt2:yyyy-MM} от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx");
        }
    }

    public class DataGet_ZPZ_EFFECTIVENESS
    {
        public List<ZPZ_EFFECTIVENESS> data { get; set; }
        public DateTime dt1 { get; set; }
        public DateTime dt2 { get; set; }
    }
}
