using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WixXmlGenerator.Models;

namespace WixXmlGenerator.Wix
{
    public static class WixXmlGenerator
    {
        public static void Generate(string sourceDir, string outputFile, string wxsDir, string projectFolderName)
        {
            try
            {
                var fileContentString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
                var folders = GetFolders(sourceDir);
                var filePaths = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

                foreach (var folder in folders)
                {
                    fileContentString += folder.ToXml();
                }

                var files = new List<Models.File>();

                foreach (var filePath in filePaths)
                {
                    files.Add(new Models.File(filePath, wxsDir, sourceDir));
                }

                foreach (var file in files)
                {
                    fileContentString += file.ToXml();
                }

                Console.WriteLine(fileContentString);
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
