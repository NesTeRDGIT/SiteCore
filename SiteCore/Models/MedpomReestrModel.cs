using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceLoaderMedpomData;
using SiteCore.Data;

namespace SiteCore.Models
{
    public class ViewReestrViewModel
    {
        public bool ConnectWCFon { get; set; } = false;
        public FilePacket FP { get; set; }
        public int Order { get; set; }
        public List<string> ListError { get; set; } = new List<string>();
    }

    public class LoadReestViewModel
    {
        public bool ConnectWCFon { get; set; } = false;
        public bool ReestrEnabled { get; set; } = false;
        public bool TypePriem { get; set; } = false;
        public bool WithSing { get; set; }
        public FILEPACK PACKET { get; set; }
        public List<ErrorItem> ListError { get; set; } = new List<ErrorItem>();
        public List<SNILS_SIGN> SNILS_SIGN { get; set; } = new List<SNILS_SIGN>();
        public string CODE_MO { get; set; }
        public string NAME_OK { get; set; }
    }
    public class ErrorItem
    {
        public ErrorItem()
        {

        }
        public ErrorItem(ErrorT _ErrorT, string _Error)
        {
            this.Error = _Error;
            ErrorT = _ErrorT;
        }
        public string Error { get; set; }
        public ErrorT ErrorT { get; set; }
    }
    public enum ErrorT
    {
        TextGreen,
        TextRed
    }

    public class EditErrorSPRViewModel
    {
        public ICollection<ErrorSPRSection> Section { get; set; }
        public ErrorSPR Error { get; set; }
    }
}
