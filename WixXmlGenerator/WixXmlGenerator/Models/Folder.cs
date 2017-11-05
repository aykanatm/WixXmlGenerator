using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WixXmlGenerator.Models
{
    public class Folder
    {
        private readonly DirectoryInfo _directoryInfo;
        private readonly List<Folder> _children;
        
        public Folder(string path, List<Folder> chidren)
        {
            _directoryInfo = new DirectoryInfo(path);
            _children = chidren;
        }

        public string ToXml()
        {
            var xmlString = "";

            if (_children.Any())
            {
                xmlString += "<Directory Id=\"" + _directoryInfo.Name + "FolderId\" Name=\"" + _directoryInfo.Name + "\">\n";
                foreach (var child in _children)
                {
                    xmlString += child.ToXml();
                }
                xmlString += "</Directory>\n";
            }
            else
            {
                xmlString = "<Directory Id=\"" + _directoryInfo.Name + "FolderId\" Name=\"" + _directoryInfo.Name + "\"/>\n";
            }

            return xmlString;
        }
    }
}
