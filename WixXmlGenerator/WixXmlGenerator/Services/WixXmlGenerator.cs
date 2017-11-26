using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WixXmlGenerator.Models;

namespace WixXmlGenerator.Services
{
    public static class WixXmlGenerator
    {
        public static string Generate(string sourceDir, string outputFile, string wxsPath, string projectFolderName)
        {
            try
            {
                var fileContentString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<Wix xmlns=\"http://schemas.microsoft.com/wix/2006/wi\">\n";
                var folders = GetFolders(sourceDir);
                var filePaths = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

                fileContentString += "<Directory Id=\"TARGETDIR\" Name=\"SourceDir\">\n";
                foreach (var folder in folders)
                {
                    fileContentString += folder.ToXml();
                }
                fileContentString +=
                    "<Directory Id=\"ProgramMenuFolder\">\n<Directory Id=\"MyShortcutsDir\" Name=\"" + projectFolderName + "\"/>\n</Directory>\n";
                fileContentString += "</Directory>\n";

                var files = new List<Models.File>();

                foreach (var filePath in filePaths)
                {
                    files.Add(new Models.File(filePath, wxsPath, sourceDir));
                }

                foreach (var file in files)
                {
                    fileContentString += file.ToXml();
                }

                fileContentString += "</Wix>";
                return fileContentString;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static List<Folder> GetFolders(string sourceDir)
        {
            var folders = new List<Folder>();

            var directoryPaths = Directory.GetDirectories(sourceDir);

            if (directoryPaths.Any())
            {
                foreach (var directoryPath in directoryPaths)
                {
                    var childFolders = GetFolders(directoryPath);
                    var folder = new Folder(directoryPath, childFolders);
                    folders.Add(folder);
                }
            }

            return folders;
        }
    }
}
