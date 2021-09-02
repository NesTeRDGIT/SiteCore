using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteCore.Data;

namespace SiteCore.Models
{
    public class GetONKListModel
    {
        public List<ONKReestr> Items { get; set; }
        public PageAreaModel Pages { get; set; }
    }
    public class PageAreaModel
    {
        public decimal PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int CountOnPage { get; set; }
    }
}
