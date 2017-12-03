using System;
using System.IO;

namespace WixXmlGenerator.Models
{
    public class File
    {
        private readonly FileInfo _fileInfo;
        private readonly string _wxsPath;
        private readonly DirectoryInfo _sourceDirInfo;

        public File(string path, string wxsPath, string sourceDir)
        {
            _fileInfo = new FileInfo(path);
            _wxsPath = wxsPath;
            _sourceDirInfo = new DirectoryInfo(sourceDir);
        }

        public string ToXml()
        {
            var xmlString = "";

            var wxsUri = new Uri(_wxsPath);
            var fileUri = new Uri(_fileInfo.FullName);
            var relativeUri = wxsUri.MakeRelativeUri(fileUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString()).Replace('/', Path.DirectorySeparatorChar);

            var directoryName = _fileInfo.Directory.Name == _sourceDirInfo.Name 
                ? "INSTALLDIR" 
                : _fileInfo.Directory.Name + "FolderId";
            var componentId = _fileInfo.Directory.Name == _sourceDirInfo.Name
                ? _fileInfo.Name.Replace("-", "_")
                : _fileInfo.Directory.Name + "_" + _fileInfo.Name.Replace("-", "_");

            xmlString += "<DirectoryRef Id=\"" + directoryName + "\">\n";
            xmlString += "<Component Id=\"" + componentId + "\">\n";
            xmlString += "<File Id=\"" + _fileInfo.Name.Replace("-", "_") + "\" Name=\"" + _fileInfo.Name + "\" Source=\"" + relativePath + "\" KeyPath=\"yes\"/>\n";
            xmlString += "</Component>\n";
            xmlString += "</DirectoryRef>\n";

            return xmlString;
        }
    }
}
