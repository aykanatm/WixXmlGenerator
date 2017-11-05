using System;
using System.IO;

namespace WixXmlGenerator.Models
{
    public class File
    {
        private readonly FileInfo _fileInfo;
        private readonly string _wxsPath;
        private readonly string _parentDirectory;

        public File(string path, string wxsPath, string sourceDir)
        {
            _fileInfo = new FileInfo(path);
            _wxsPath = wxsPath;
            _parentDirectory = new DirectoryInfo(sourceDir).Name;
        }

        public string ToXml()
        {
            var xmlString = "";

            var wxsUri = new Uri(_wxsPath);
            var fileUri = new Uri(_fileInfo.FullName);
            var diff = wxsUri.MakeRelativeUri(fileUri);
            var source = diff.OriginalString.Replace("/","\\");

            var directoryName = _fileInfo.Directory.Name == _parentDirectory
                ? "INSTALLDIR"
                : _fileInfo.Directory.Name + "FolderId";

            xmlString += "<DirectoryRef Id=\"" + directoryName + "\">\n";
            xmlString += "<Component Id=\"" + _fileInfo.Name.Replace("-", "_") + "\">\n";
            xmlString += "<File Id=\"" + _fileInfo.Name.Replace("-", "_") + "\" Name=\"" + _fileInfo.Name + "\" Source=\"" + source + "\" KeyPath=\"yes\"/>\n";
            xmlString += "</Component>\n";
            xmlString += "</DirectoryRef>\n";

            return xmlString;
        }
    }
}
