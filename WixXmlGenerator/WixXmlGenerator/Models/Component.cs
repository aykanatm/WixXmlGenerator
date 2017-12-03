using System.IO;

namespace WixXmlGenerator.Models
{
    public class Component
    {
        private readonly string _id;

        public Component(string filePath, string sourceDir)
        {
            var fileInfo = new FileInfo(filePath);
            var sourceDirectoryInfo = new DirectoryInfo(sourceDir);
            var parentDirectoryInfo = fileInfo.Directory;
            if (parentDirectoryInfo != null)
            {
                _id = parentDirectoryInfo.Name == sourceDirectoryInfo.Name 
                    ? fileInfo.Name 
                    : parentDirectoryInfo.Name + "_" + fileInfo.Name;
            }
        }
        public string ToXml()
        {
            var xmlString = "<ComponentRef Id=\"" + _id + "\"/>\n";

            return xmlString;
        }
    }
}
