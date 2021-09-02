using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelManager;
using Microsoft.AspNetCore.Authorization;
using SiteCore.Data;
using SiteCore.Class;

namespace SiteCore.Controllers
{
    [Authorize(Roles = "STAT_STAC")]
    public class STAC_PLANController : Controller
    {

        private MyOracleSet myOracleSet { get;  }
        private ISTAC_PLANExcelCreator StacPlanExcelCreator { get; }

        public STAC_PLANController(MyOracleSet myOracleSet, ISTAC_PLANExcelCreator StacPlanExcelCreator)
        {
            this.myOracleSet = myOracleSet;
            this.StacPlanExcelCreator = StacPlanExcelCreator;
        }

        public IActionResult STAC_PLAN()
        {
            return View();
        }

        #region TAB3

        [HttpGet]
        public IActionResult GET_TABLE_3()
        {
            var tbl = myOracleSet.STAT_STAC_TBL_3();
            TempData["TABLE_3"] = tbl.ObjectToXml();
            return PartialView("_Tab3_Partical", tbl);
        }

        [HttpGet]
        public IActionResult GET_TABLE_3_XLS()
        {
            var tbl = TempData["TABLE_3"].ToString().XmlToObject<DataTable>();
            TempData["TABLE_3"] = tbl.ObjectToXml();
            var t = $"от {DateTime.Now:dd-MM-yyyy}";
            if (tbl.Columns.Contains("COMM") && tbl.Rows.Count > 0)
            {
                t = tbl.Rows[0]["comm"].ToString();
            }
            return File(StacPlanExcelCreator.CreateXLS_TABLE_3(tbl), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Таблица 3 - {t}.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> BUILD_TABLE_3(DateTime dt1, DateTime dt2)
        {
            try
            {
                if (dt1 > dt2)
                    return StatusCode(410, "Дата_1 > Дата_2");
                if (dt1.Year != dt2.Year)
                    return StatusCode(410, "Сбор возможен только в пределах года");
                await myOracleSet.BUILD_STAT_STAC_TBL_3(dt1, dt2);
                return Json(true);
            }
            catch (Exception ex)
            {
                return StatusCode(410, ex.Message);
            }

        }
        #endregion

        #region TAB2

        [HttpGet]
        public IActionResult GET_TABLE_2()
        {
            var tbl = myOracleSet.STAT_STAC_TBL_2();
            tbl = StacPlanExcelCreator.PIVOT_TABLE_2(tbl);
            TempData["TABLE_2"] = tbl.ObjectToXml();
            return PartialView("_Tab2_Partical", tbl);
        }

        [HttpGet]
        public IActionResult GET_TABLE_2_XLS()
        {
            var tbl = TempData["TABLE_2"].ToString().XmlToObject<DataTable>();
            TempData["TABLE_2"] = tbl.ObjectToXml();
            var t = $"от {DateTime.Now:dd-MM-yyyy}";
            if (tbl.Columns.Contains("COMM") && tbl.Rows.Count > 0)
            {
                t = tbl.Rows[0]["comm"].ToString();
            }
            return File(StacPlanExcelCreator.CreateXLS_TABLE_2(tbl), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Таблица 2 - {t}.xlsx");
        }
        [HttpPost]
        public async Task<IActionResult> BUILD_TABLE_2(DateTime dt1, DateTime dt2)
        {
            try
            {
                if (dt1 > dt2)
                    return StatusCode(410, "Дата_1 > Дата_2");
                if (dt1.Year != dt2.Year)
                    return StatusCode(410, "Сбор возможен только в пределах года");
                await myOracleSet.BUILD_STAT_STAC_TBL_2(dt1, dt2);
                return Json(true);
            }
            catch (Exception ex)
            {
                return StatusCode(410, ex.Message);
            }

        }

        #endregion

        #region TAB1

        [HttpGet]
        public IActionResult GET_TABLE_1()
        {
            var tbl = myOracleSet.STAT_STAC_TBL_1();
            TempData["TABLE_1"] = tbl.ObjectToXml();
            return PartialView("_Tab1_Partical", tbl);
        }

        [HttpGet]
        public IActionResult GET_TABLE_1_XLS()
        {
            var tbl = TempData["TABLE_1"].ToString().XmlToObject<DataTable>(); ;
            TempData["TABLE_1"] = tbl.ObjectToXml();
            var t = $"от {DateTime.Now:dd-MM-yyyy}";
            if (tbl.Columns.Contains("COMM") && tbl.Rows.Count > 0)
            {
                t = tbl.Rows[0]["comm"].ToString();
            }
            return File(StacPlanExcelCreator.CreateXLS_TABLE_1(tbl), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Таблица 1 - {t}.xlsx");
        }
        
        [HttpPost]
        public async Task<IActionResult> BUILD_TABLE_1(DateTime dt1, DateTime dt2)
        {
            try
            {
                if (dt1 > dt2)
                    return StatusCode(410, "Дата_1 > Дата_2");
                if (dt1.Year != dt2.Year)
                    return StatusCode(410, "Сбор возможен только в пределах года");
                await myOracleSet.BUILD_STAT_STAC_TBL_1(dt1, dt2);
                return Json(true);
            }
            catch (Exception ex)
            {
                return StatusCode(410, ex.Message);
            }
        }

        #endregion

        #region TAB4

        [HttpGet]
        public IActionResult GET_TABLE_4()
        {
            var tbl = myOracleSet.STAT_STAC_TBL_4();
            TempData["TABLE_4"] = tbl.ObjectToXml();
            return PartialView("_Tab4_Partical", tbl);
        }

        [HttpGet]
        public IActionResult GET_TABLE_4_XLS()
        {
            var tbl = TempData["TABLE_4"].ToString().XmlToObject<DataTable>();
            TempData["TABLE_4"] = tbl.ObjectToXml();
            var t = $"от {DateTime.Now:dd-MM-yyyy}";
            if (tbl.Columns.Contains("COMM") && tbl.Rows.Count > 0)
            {
                t = tbl.Rows[0]["comm"].ToString();
            }
            return File(StacPlanExcelCreator.CreateXLS_TABLE_4(tbl), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Таблица 4 - {t}.xlsx");
        }
        [HttpPost]
        public async Task<IActionResult> BUILD_TABLE_4(DateTime dt1, DateTime dt2)
        {
            try
            {
                if (dt1 > dt2)
                    return StatusCode(410, "Дата_1 > Дата_2");
                if (dt1.Year != dt2.Year)
                    return StatusCode(410, "Сбор возможен только в пределах года");
                await myOracleSet.BUILD_STAT_STAC_TBL_4(dt1, dt2);
                return Json(true);
            }
            catch (Exception ex)
            {
                return StatusCode(410, ex.Message);
            }

        }

        #endregion
    }
}
