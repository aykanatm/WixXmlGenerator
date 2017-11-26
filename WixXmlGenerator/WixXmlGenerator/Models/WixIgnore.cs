using System;
using System.Collections.Generic;

namespace WixXmlGenerator.Models
{
    public class WixIgnore
    {
        private readonly List<string> _filePaths;
        private readonly List<string> _folderPaths;

        public WixIgnore(List<string> filePaths, List<string> folderPaths)
        {
            _filePaths = filePaths;
            _folderPaths = folderPaths;
        }

        public bool Contains(string path)
        {
            try
            {
                var result = _filePaths.Contains(path) || _folderPaths.Contains(path);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
