using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ServiceLoaderMedpomData;
using SiteCore.Controllers;
using SiteCore.Data;
using System.Data;
namespace SiteCore.Class
{
    public static class ServiceLoaderMedpomDataExtensions
    {
        public static string RusName(this TYPEFILE t)
        {
            return t switch
            {
                TYPEFILE.DD => "Медицинские осмотры несовершеннолетних(предварительные)",
                TYPEFILE.DF => "Медицинские осмотры несовершеннолетних(профилактические)",
                TYPEFILE.DO => "Профилактические осмотры взрослого населения",
                TYPEFILE.DP => "Первый этап диспансеризации",
                TYPEFILE.DR => "Медицинские осмотры несовершеннолетних(периодические)",
                TYPEFILE.DS => "Диспансеризация детей-сирот",
                TYPEFILE.DU => "Диспансеризация опекаемых",
                TYPEFILE.DV => "Второй этап диспансеризации",
                TYPEFILE.DA => "Первый этап углубленной диспансеризации",
                TYPEFILE.DB => "Второй этап углубленной диспансеризации",
                TYPEFILE.H => "Оказанная медицинская помощь",
                TYPEFILE.LD => "Файл персональных данных для файла DD",
                TYPEFILE.LF => "Файл персональных данных для файла DF",
                TYPEFILE.LH => "Файл персональных данных для файла H",
                TYPEFILE.LO => "Файл персональных данных для файла DO",
                TYPEFILE.LP => "Файл персональных данных для файла DP",
                TYPEFILE.LR => "Файл персональных данных для файла DR",
                TYPEFILE.LS => "Файл персональных данных для файла DS",
                TYPEFILE.LT => "Файл персональных данных для файла T",
                TYPEFILE.LU => "Файл персональных данных для файла DU",
                TYPEFILE.LV => "Файл персональных данных для файла DV",
                TYPEFILE.T => "Высокотехнологичная помощь",
                TYPEFILE.C => "Оказанная медицинская помощь при ЗНО",
                TYPEFILE.LC => "Файл персональных данных для файла C",
                TYPEFILE.LA => "Файл персональных данных для файла DA",
                TYPEFILE.LB => "Файл персональных данных для файла DB",
                _ => ""
            };
        }

        public static string RusName(this FileType t)
        {
            return t switch
            {
                FileType.DD => "Медицинские осмотры несовершеннолетних(предварительные)",
                FileType.DF => "Медицинские осмотры несовершеннолетних(профилактические)",
                FileType.DO => "Профилактические осмотры взрослого населения",
                FileType.DP => "Первый этап диспансеризации",
                FileType.DR => "Медицинские осмотры несовершеннолетних(периодические)",
                FileType.DS => "Диспансеризация детей-сирот",
                FileType.DU => "Диспансеризация опекаемых",
                FileType.DV => "Второй этап диспансеризации",
                FileType.DA => "Первый этап углубленной диспансеризации",
                FileType.DB => "Второй этап углубленной диспансеризации",
                FileType.H => "Оказанная медицинская помощь",
                FileType.LD => "Файл персональных данных для файла DD",
                FileType.LF => "Файл персональных данных для файла DF",
                FileType.LH => "Файл персональных данных для файла H",
                FileType.LO => "Файл персональных данных для файла DO",
                FileType.LP => "Файл персональных данных для файла DP",
                FileType.LR => "Файл персональных данных для файла DR",
                FileType.LS => "Файл персональных данных для файла DS",
                FileType.LT => "Файл персональных данных для файла T",
                FileType.LU => "Файл персональных данных для файла DU",
                FileType.LV => "Файл персональных данных для файла DV",
                FileType.T => "Высокотехнологичная помощь",
                FileType.C => "Оказанная медицинская помощь при ЗНО",
                FileType.LC => "Файл персональных данных для файла C",
                FileType.LA => "Файл персональных данных для файла DA",
                FileType.LB => "Файл персональных данных для файла DB",
                _ => ""
            };
        }


        public static string RusName(this StepsProcess t)
        {
            return t switch
            {
                StepsProcess.NotInvite => "Первичная проверка не пройдена",
                StepsProcess.Invite => "Первичная проверка пройдена",
                StepsProcess.XMLxsd => "Схема верна",
                StepsProcess.ErrorXMLxsd => "Ошибка схемы документа",
                StepsProcess.FlkErr => "Ошибка при загрузке файла",
                StepsProcess.FlkOk => "Проверка завершена",
                _ => ""
            };
        }

        public static string RusName(this STATUS_FILE t)
        {
            return t switch
            {
                STATUS_FILE.INVITE => "Принят к проверке",
                STATUS_FILE.NOT_INVITE => "Не принят к проверке",
                STATUS_FILE.XML_NOT_VALID => "Ошибка схемы документа",
                STATUS_FILE.XML_VALID => "Схема верна",
                _ => ""
            };
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


        public static List<string> GetErrors(this ModelStateDictionary model)
        {
            return (from modelState in model.Values from error in modelState.Errors select $"{error.ErrorMessage}{error.Exception?.Message}").ToList();
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

        public static bool Between(this DateTime? value, DateTime dt1, DateTime dt2)
        {
            if (!value.HasValue) return false;
            return value.Value.Between(dt1,dt2);
        }

        public static bool Between(this DateTime value, DateTime dt1, DateTime dt2)
        {
            return value >= dt1 && value <= dt2;
        }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }

    public static  class SerializationExtensions
    {
        public static string ObjectToXml(this object obj)
        {
            if (obj is DataTable TBL)
            {
                if (string.IsNullOrEmpty(TBL.TableName))
                    TBL.TableName = "serilization";
            }
            var ser = new XmlSerializer(obj.GetType());
            using var ms = new MemoryStream();
            ser.Serialize(ms, obj);
            return ms.ReadToEnd();
        }

        public static T XmlToObject<T>(this string val)
        {
            var ser = new XmlSerializer(typeof(T));
            using var xmlreader = XmlReader.Create(new StringReader(val));
            return (T) ser.Deserialize(xmlreader);
        }
    }


   

}
