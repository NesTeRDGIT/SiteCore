using System;
using System.Collections.Generic;

namespace SiteCore.Models
{
    public class ErrorViewModel
    {
        public List<string> Errors { get; set; } =    new List<string>();
        public string returnURL { get; set; } = "";
    }
}
