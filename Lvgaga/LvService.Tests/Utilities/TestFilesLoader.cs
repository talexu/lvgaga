using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace LvService.Tests.Utilities
{
    public class TestFilesLoader
    {
        private string[] _folders;
        private string _dir;

        public TestFilesLoader()
        {

        }

        public void InitializeFolder(string[] newFolders)
        {
            _folders = newFolders;
            var baseDir = Directory.GetParent(Environment.CurrentDirectory).Parent;
            if (baseDir == null) return;

            var folders = new List<string>(_folders);
            folders.Insert(0, baseDir.FullName);
            _dir = Path.Combine(folders.ToArray());
        }

        public string ReadAllText(string fileName)
        {
            return File.ReadAllText(Path.Combine(_dir, fileName));
        }
    }
}