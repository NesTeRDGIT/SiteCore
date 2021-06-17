using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiteCore.Data
{
    public interface IMedpomRepository
    {
        Task<string> AddFile(string CODE_MO, string Name, byte[] file);
        Task<string> AddFile(string CODE_MO, string Name, Stream file);
        string GetPath(string CODE_MO, string Name);
        string GetDir(string CODE_MO);
        void DeleteFile(string CODE_MO, string Name);
        void Clear(string CODE_MO);
    }

    public class MedpomFileManager: IMedpomRepository
    {
        public string SharedFolder { get; }

        public MedpomFileManager(string SharedFolder)
        {
            this.SharedFolder = SharedFolder;
        }

        public async Task<string> AddFile(string CODE_MO, string Name, byte[] file)
        {
            await using var ms = new MemoryStream(file);
            return await AddFile(CODE_MO, Name, ms);
        }
        public async Task<string> AddFile(string CODE_MO, string Name, Stream file)
        {
            var path = GetPath(CODE_MO, Name);
            CheckDir(Path.GetDirectoryName(path));
            await using var st = File.Create(path);
            await file.CopyToAsync(st);
            st.Close();
            return path;
        }
        public void DeleteFile(string CODE_MO, string Name)
        {
            var path = GetPath(CODE_MO, Name);
            if(File.Exists(path))
                File.Delete(path);
        }

        public void Clear(string CODE_MO)
        {
            var dir = GetDir(CODE_MO);
            if(Directory.Exists(dir))
                Directory.Delete(dir, true);
        }

        public string GetPath(string CODE_MO,string Name)
        {
            return Path.Combine(GetDir(CODE_MO), Name);
        }
        public string GetDir(string CODE_MO)
        {
            return Path.Combine(SharedFolder, CODE_MO);
        }
        private void CheckDir(string Dir)
        {
            if (!Directory.Exists(Dir))
            {
                Directory.CreateDirectory(Dir);
            }
        }
    }
}
