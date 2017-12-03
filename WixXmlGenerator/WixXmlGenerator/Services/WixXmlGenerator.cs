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
                var wixIgnore = WixIgnoreParser.GenerateWixIgnore(sourceDir);
                var filePaths = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
                var folders = GetFolders(sourceDir, wixIgnore);

                fileContentString += "<!-- Folder Structure -->\n";
                fileContentString += "<Directory Id=\"TARGETDIR\" Name=\"SourceDir\">\n";
                foreach (var folder in folders)
                {
                    fileContentString += folder.ToXml();
                }
                fileContentString +=
                    "<Directory Id=\"ProgramMenuFolder\">\n<Directory Id=\"MyShortcutsDir\" Name=\"" + projectFolderName + "\"/>\n</Directory>\n";
                fileContentString += "</Directory>\n";

                var files = new List<Models.File>();
                var components = new List<Component>();

                foreach (var filePath in filePaths)
                {
                    if (!wixIgnore.Contains(filePath))
                    {
                        files.Add(new Models.File(filePath, wxsPath, sourceDir));
                        components.Add(new Component(filePath, sourceDir));
                    }
                }

                fileContentString += "<!-- Files -->\n";
                foreach (var file in files)
                {
                    fileContentString += file.ToXml();
                }

                fileContentString += "<!-- Application Components -->\n";
                fileContentString += "<Feature Id=\"ProductFeature\" Title=\"" + projectFolderName + "\" Level=\"1\">\n";
                foreach (var component in components)
                {
                    fileContentString += component.ToXml();
                }
                fileContentString += "</Feature>\n";
                fileContentString += "</Wix>";
                return fileContentString;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static List<Folder> GetFolders(string sourceDir, WixIgnore wixIgnore)
        {
            var folders = new List<Folder>();

            var directoryPaths = Directory.GetDirectories(sourceDir);

            if (directoryPaths.Any())
            {
                foreach (var directoryPath in directoryPaths)
                {
                    if (!wixIgnore.Contains(directoryPath))
                    {
                        var childFolders = GetFolders(directoryPath, wixIgnore);
                        var folder = new Folder(directoryPath, childFolders);
                        folders.Add(folder);
                    }
                }
            }

            return folders;
        }
    }
}
