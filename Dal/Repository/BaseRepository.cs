using System;
using System.IO;
using Dal.Helper;
using Dal.Model;

namespace Dal.Repository
{
    public abstract class BaseRepository<T> where  T : BaseModel
    {
        private readonly string _applicationPath = Environment.CurrentDirectory;
        private readonly string _folderName;

        protected BaseRepository(string folder)
        {
            _folderName = folder;

            var folderPass = Path.Combine(_applicationPath, _folderName);
            if (!Directory.Exists(folderPass))
            {
                Directory.CreateDirectory(folderPass);
            }

        }

        public T GetJson(long id)
        {
            var path = GetPath(id);
            if (!File.Exists(path))
            {
                return null;
            }

            using (var sr = new StreamReader(path))
            {
                var json = sr.ReadToEnd();
                var model = JsonSerializeHelper.Deserialize<T>(json);
                return model;
            }
        }

        public void SaveJson(T model)
        {
            var path = GetPath(model.Id);
            var json = JsonSerializeHelper.Serialize(model);

            using (var sw = new StreamWriter(path))
            {
                sw.Write(json);
            }
        }

        private string GetPath(long id)
        {
            var fileName = $"{id}.json";
            return Path.Combine(_applicationPath, _folderName, fileName);
        }

    }
}
