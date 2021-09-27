using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLoaderMedpomData;
using SiteCore.Models;

namespace SiteCore.Class
{

    public interface IZipArchiver
    {
         Task<List<ZipArchiverItem>> UnZip(string filepath);
         byte[] Zip(params ZipArchiverEntry[] files);
    }
    public class ZipArchiverItem
    {
        public ZipArchiverItem(string filePath, string error)
        {
            this.FilePath = FilePath;
            this.Error = Error;
        }
        public string FilePath { get; set; }
        public string Error { get; set; }
    }
    public class ZipArchiverEntry
    {
        public ZipArchiverEntry(string FileName, byte[] Data)
        {
            this.FileName = FileName;
            this.Data = Data;
        }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
    public class ZipArchiver: IZipArchiver
    {
        public async Task<List<ZipArchiverItem>> UnZip(string filepath)
        {
            var DirFile = Path.GetDirectoryName(filepath);
            if (string.IsNullOrEmpty(DirFile) || string.IsNullOrEmpty(filepath))
                throw new Exception("Имя файла не указано");

          
            //Распаковываем во временный каталог
            var tmp = Path.Combine(Path.GetDirectoryName(filepath) ?? throw new InvalidOperationException("Не удалось получить путь к каталогу файла"), Path.GetRandomFileName());
            if (!Directory.Exists(tmp))
            {
                Directory.CreateDirectory(tmp);
            }
            // 
            var RES = await FilesHelper.FilesExtract(filepath, tmp);
            if (RES.Result == false)
            {
                Directory.Delete(tmp, true);
                throw new InvalidOperationException($"Ошибка при распаковке файла {Path.GetFileName(filepath)}");
            }
            var ErrList = new List<ZipArchiverItem>();
            //Переносим в базовый
            foreach (var f1 in Directory.GetFiles(tmp))
            {
                var zai = new ZipArchiverItem(f1,"");
                ErrList.Add(zai);
                var ext = Path.GetExtension(f1).ToUpper();
                if (ext != ".ZIP" && ext != ".XML")
                {
                    zai.FilePath = "";
                    zai.Error = $"Файл {Path.GetFileName(f1)} имеет неверный формат. Файл не загружен!";
                    continue;
                }
                var newpath = FilesHelper.MoveFileTo(f1, Path.Combine(DirFile, Path.GetFileName(f1)));
                if (!string.Equals(newpath, Path.Combine(filepath, Path.GetFileName(f1)), StringComparison.CurrentCultureIgnoreCase))
                {
                    zai.Error = $"Файл {Path.GetFileName(f1)} переименован в {Path.GetFileName(newpath)}";
                }
            }
            Directory.Delete(tmp, true);
            return ErrList;
        }

        public byte[] Zip(params ZipArchiverEntry[] files)
        {
            using var st = new MemoryStream();
            var zip = new ZipArchive(st, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                var entry = zip.CreateEntry(file.FileName);
                using var entryStream = entry.Open();
                entryStream.Write(file.Data);
                entryStream.Close();
            }
            zip.Dispose();
            return st.ToArray();
        }
    }
}
