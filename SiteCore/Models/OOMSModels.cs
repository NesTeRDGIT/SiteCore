using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteCore.Data;

namespace SiteCore.Models
{
    public class ECOViewModel
    {
        public List<ECO_MTR_Row> ECO_MTR { get; set; }
        public List<ECO_MP_Row> ECO_MP { get; set; }

        public List<ECO_MP_Row> SVOD_ECO_MO
        {
            get
            {
                var groupBy = ECO_MP.GroupBy(x => new { x.SMO, x.KSLP_NAME, x.KSLP });
                return groupBy.Select(x => new ECO_MP_Row { SMO = x.Key.SMO, KSLP = x.Key.KSLP, KSLP_NAME = x.Key.KSLP_NAME, SUMV = x.Sum(y => y.SUMV), SUMP = x.Sum(y => y.SUMP ?? 0), SLUCH_ID = x.Count() }).ToList();
            }

        }
    }
}
