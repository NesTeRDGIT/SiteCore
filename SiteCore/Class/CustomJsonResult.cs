using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SiteCore.Class
{
    public class CustomJsonResult : JsonResult
    {
        private const string _dateFormat = "yyyy-MM-ddTHH:mm:ss";

        public bool Result { get; set; } = false;
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json;charset=UTF-8";
            var isoConvert = new IsoDateTimeConverter { DateTimeFormat = _dateFormat };
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Result, Value }, isoConvert));
            response.Body.WriteAsync(bytes);
        }
        public static CustomJsonResult Create(object value, bool Result = true)
        {
            return new (value) { Result = Result };
        }
        public CustomJsonResult(object value) : base(value)
        {
           
        }


        public override Task ExecuteResultAsync(ActionContext context)
        {
            return Task.Run(() =>
            {
                ExecuteResult(context);
            });
        }
    }




}
