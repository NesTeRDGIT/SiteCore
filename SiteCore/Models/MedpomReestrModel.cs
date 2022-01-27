using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ServiceLoaderMedpomData;
using SiteCore.Class;
using SiteCore.Data;

namespace SiteCore.Models
{

    public class LoadReestViewModel
    {
        public bool ConnectWCFon { get; set; } = false;
        public bool ReestrEnabled { get; set; } = false;
        public bool TypePriem { get; set; } = false;
        public bool WithSing { get; set; }
        public string CODE_MO { get; set; }
        public string NAME_OK { get; set; }
        public List<ErrorItem> ListError { get; set; } = new();
        public List<FileItem> FileList { get; set; } = new();
        public List<SNILS_SIGN> SNILS_SIGN { get; set; } = new();

    }


    public class  FileItemBase
    {
        public  int ID { get; set; }
        public STATUS_FILE STATUS { get; set; }
        public string STATUS_NAME => STATUS.RusName();
        public string  FILENAME { get; set; }
        public TYPEFILE? TYPE_FILE { get; set; }
        public string TYPE_FILE_NAME => TYPE_FILE?.RusName();
        public string  COMENT { get; set; }
    }

    public class FileItem : FileItemBase
    {
        public FileItemBase FILE_L { get; set; }
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
        TextGreen = 0,
        TextRed = 1
    }
    

    public class EditErrorSPRViewModel
    {
        public List<SectionSprModel> Sections { get; set; }

        public List<ErrorSprModel> Top30 => Sections.SelectMany(x => x.Errors).Where(x => (DateTime.Now - x.D_EDIT).Days <= 30).ToList();
    }
    public class SectionSprModel
    {
      public string   SECTION_NAME { get; set; }
      public int ID_SECTION { get; set; }
      public List<ErrorSprModel> Errors { get; set; }
    }


    public class ErrorSprModel
    {
        public int? ID_ERR { get; set; }
        public DateTime D_EDIT { get; set; }
        [Required(ErrorMessage ="Код ошибки обязателен к заполнению")]
        public string OSN_TFOMS{ get; set; }
        [Required(ErrorMessage = "Пример ошибки обязателен к заполеннию")]
        public string EXAMPLE{ get; set; }
        [Required(ErrorMessage = "Текст ошибки обязателен к заполнению")]
        public string TEXT { get; set; }
        [Required(ErrorMessage = "Секция обязательна к заполению")]
        public int? ID_SECTION { get; set; }
        [Required(ErrorMessage = "Дата начала обязательна к заполению")]
        public DateTime D_BEGIN { get; set; }      
        public DateTime? D_END { get; set; }
        [Required(ErrorMessage = "Признак МЭК обязателен к заполнению")]
        public bool ISMEK { get; set; }
    }


    public class ViewReestrModel
    {
        public bool ConnectWCFon { get; set; } = false;
        public FilePacketNew FP { get; set; }

     
    }

    public class FilePacketNew
    {
       public string CodeMO { get; set; }
        public string CaptionMO { get; set; }
        public DateTime Date { get; set; }
        public IST IST { get; set; }
        public string CommentSite { get; set; }
        public int Order { get; set; }
        public string WARNNING { get; set; }
        public bool isResult { get; set; }
        public StatusFilePack Status { get; set; }

        public List<FileView> FileList { get; set; }

    }


    public class FileViewBase
    {
        public string FileName { get; set; }
        public StepsProcess Process { get; set; }
        public FileType? Type { get; set; }
        public string TYPE_NAME => Type?.RusName();
        public string Comment { get; set; }
    }
    public class FileView: FileViewBase
    {
        public FileViewBase FILE_L { get; set; }
    }



}
