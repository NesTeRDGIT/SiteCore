using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ServiceLoaderMedpomData;
using SiteCore.Data;

namespace SiteCore.Class
{
    public static class ServiceLoaderMedpomDataExtensions
    {
        public static string RusName(this TYPEFILE t)
        {
            switch (t)
            {
                case TYPEFILE.DD: return "Медицинские осмотры несовершеннолетних(предварительные)";
                case TYPEFILE.DF: return "Медицинские осмотры несовершеннолетних(профилактические)";
                case TYPEFILE.DO: return "Профилактические осмотры взрослого населения";
                case TYPEFILE.DP: return "Первый этап диспансеризации";
                case TYPEFILE.DR: return "Медицинские осмотры несовершеннолетних(периодические)";
                case TYPEFILE.DS: return "Диспансеризация дитей-сирот";
                case TYPEFILE.DU: return "Диспансеризация опекаемых";
                case TYPEFILE.DV: return "Второй этап диспансеризации";
                case TYPEFILE.H: return "Оказанная медицинская помощь";
                case TYPEFILE.LD: return "Файл персональных данных для файла DD";
                case TYPEFILE.LF: return "Файл персональных данных для файла DF";
                case TYPEFILE.LH: return "Файл персональных данных для файла H";
                case TYPEFILE.LO: return "Файл персональных данных для файла DO";
                case TYPEFILE.LP: return "Файл персональных данных для файла DP";
                case TYPEFILE.LR: return "Файл персональных данных для файла DR";
                case TYPEFILE.LS: return "Файл персональных данных для файла DS";
                case TYPEFILE.LT: return "Файл персональных данных для файла T";

                case TYPEFILE.LU: return "Файл персональных данных для файла DU";
                case TYPEFILE.LV: return "Файл персональных данных для файла DV";
                case TYPEFILE.T: return "Высокотехнологичная помощь";
                case TYPEFILE.C: return "Оказанная медицинская помощь при ЗНО";
                case TYPEFILE.LC: return "Файл персональных данных для файла C";
                default:
                    return "";
            }
        }

        public static string RusName(this ServiceLoaderMedpomData.FileType t)
        {
            switch (t)
            {
                case ServiceLoaderMedpomData.FileType.DD: return "Медицинские осмотры несовершеннолетних(предварительные)";
                case ServiceLoaderMedpomData.FileType.DF: return "Медицинские осмотры несовершеннолетних(профилактические)";
                case ServiceLoaderMedpomData.FileType.DO: return "Профилактические осмотры взрослого населения";
                case ServiceLoaderMedpomData.FileType.DP: return "Первый этап диспансеризации";
                case ServiceLoaderMedpomData.FileType.DR: return "Медицинские осмотры несовершеннолетних(периодические)";
                case ServiceLoaderMedpomData.FileType.DS: return "Диспансеризация дитей-сирот";
                case ServiceLoaderMedpomData.FileType.DU: return "Диспансеризация опекаемых";
                case ServiceLoaderMedpomData.FileType.DV: return "Второй этап диспансеризации";
                case ServiceLoaderMedpomData.FileType.H: return "Оказанная медицинская помощь";
                case ServiceLoaderMedpomData.FileType.LD: return "Файл персональных данных для файла DD";
                case ServiceLoaderMedpomData.FileType.LF: return "Файл персональных данных для файла DF";
                case ServiceLoaderMedpomData.FileType.LH: return "Файл персональных данных для файла H";
                case ServiceLoaderMedpomData.FileType.LO: return "Файл персональных данных для файла DO";
                case ServiceLoaderMedpomData.FileType.LP: return "Файл персональных данных для файла DP";
                case ServiceLoaderMedpomData.FileType.LR: return "Файл персональных данных для файла DR";
                case ServiceLoaderMedpomData.FileType.LS: return "Файл персональных данных для файла DS";
                case ServiceLoaderMedpomData.FileType.LT: return "Файл персональных данных для файла T";

                case ServiceLoaderMedpomData.FileType.LU: return "Файл персональных данных для файла DU";
                case ServiceLoaderMedpomData.FileType.LV: return "Файл персональных данных для файла DV";
                case ServiceLoaderMedpomData.FileType.T: return "Высокотехнологичная помощь";
                case ServiceLoaderMedpomData.FileType.C: return "Оказанная медицинская помощь при ЗНО";
                case ServiceLoaderMedpomData.FileType.LC: return "Файл персональных данных для файла C";
                default:
                    return "";
            }
        }


        public static string RusName(this ServiceLoaderMedpomData.StepsProcess t)
        {
            switch (t)
            {
                case ServiceLoaderMedpomData.StepsProcess.NotInvite: return "Первичная проверка не пройдена";
                case ServiceLoaderMedpomData.StepsProcess.Invite: return "Первичная проверка пройдена";
                case ServiceLoaderMedpomData.StepsProcess.XMLxsd: return "Схема верна";
                case ServiceLoaderMedpomData.StepsProcess.ErrorXMLxsd: return "Ошибка схемы документа";
                case ServiceLoaderMedpomData.StepsProcess.FlkErr: return "Ошибка при загрузке файла";
                case ServiceLoaderMedpomData.StepsProcess.FlkOk: return "Проверка завершена";
                default:
                    return "";
            }
        }

        public static string RusName(this STATUS_FILE t)
        {
            switch (t)
            {
                case STATUS_FILE.INVITE: return "Принят к проверке";
                case STATUS_FILE.NOT_INVITE: return "Не принят к проверке";
                case STATUS_FILE.XML_NOT_VALID: return "Ошибка схемы документа";
                case STATUS_FILE.XML_VALID: return "Схема верна";
                default:
                    return "";
            }
        }

        public static string ToString(this DateTime? dt, string format)
        {
            return dt.HasValue ? dt.Value.ToString(format) : "";
        }

        public static string ToJson(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);


        }

    }


    public static class ControllerContextEx
    {
        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }
            controller.ViewData.Model = model;
            await using var writer = new StringWriter();
            IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            if (viewEngine != null)
            {
                var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }


                var viewContext = new  ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer, new HtmlHelperOptions());
                await viewResult.View.RenderAsync(viewContext);
            }
            return writer.GetStringBuilder().ToString();
        }
    }

    public static class Ext
    {
        public static string ToHTMLStr(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToHTMLStr() : "";
        }

        public static string ToHTMLStr(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }


        public static string ToStr(this DateTime? value, string Format)
        {
            return value.HasValue ? value.Value.ToString(Format) : "";
        }

        public static string ToYesNo(this bool value)
        {
            return value ? "Да" : "Нет";
        }

        public static string ToYesNo(this bool? value)
        {

            return value.HasValue ? ToYesNo(value.Value) : "";
        }

        public static string ToOSN(this bool value)
        {
            return value ? "Обоснованно" : "Необоснованно";
        }
        public static string ToOSN(this bool? value)
        {
            return value.HasValue ? ToOSN(value.Value) : "";
        }
      /*  public static string ToStr(this ExpertTip ex)
        {
            switch (ex)
            {
                case ExpertTip.EKMP: return "ЭКМП";
                case ExpertTip.MEE: return "МЭЭ";
                case ExpertTip.MEK: return "МЭК";
                default: return "";
            }

        }*/

        public static void RemoveRange<T>(this ICollection<T> list, int From, int To)
        {
            var del = new List<T>();
            var items = list.ToList();
            for (var i = From; i <= To; i++)
            {
                del.Add(items[i]);
            }

            foreach (var d in del)
            {
                list.Remove(d);
            }

        }

        public static bool In(this StatusCS_LIST val, params StatusCS_LIST[] values)
        {
            return values.Contains(val);
        }


    }



}
