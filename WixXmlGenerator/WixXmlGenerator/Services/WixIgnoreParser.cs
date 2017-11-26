using System;
using System.Collections.Generic;
using System.IO;
using WixXmlGenerator.Models;

namespace WixXmlGenerator.Services
{
    public static class WixIgnoreParser
    {
        public static WixIgnore GenerateWixIgnore(string sourceDir)
        {
            try
            {
                var wixIgnoreFilePath = AppDomain.CurrentDomain.BaseDirectory + ".wixignore";
                if (!System.IO.File.Exists(wixIgnoreFilePath))
                {
                    System.IO.File.Create(wixIgnoreFilePath);
                }

                var filePaths = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
                var folderPaths = Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories);

                var ignoredfilePaths = new List<string>();
                var ignoredfolderPaths = new List<string>();

                using (var sr = new StreamReader(wixIgnoreFilePath))
                {
                    string line = null;
                    
                    do
                    {
                        line = sr.ReadLine();
                        if (line != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                // Extension
                                if (line.StartsWith("*."))
                                {
                                    var extension = line.Substring(2, 3);

                                    foreach (var filePath in filePaths)
                                    {
                                        if (filePath.EndsWith(extension))
                                        {
                                            ignoredfilePaths.Add(filePath);
                                        }
                                    }
                                }
                                // Directory
                                else if (line.StartsWith("/"))
                                {
                                    if (line.EndsWith("/"))
                                    {
                                        foreach (var folderPath in folderPaths)
                                        {
                                            var directoryInfo = new DirectoryInfo(folderPath);
                                            if (directoryInfo.Exists && directoryInfo.Name == line.Substring(1, line.Length - 2))
                                            {
                                                ignoredfolderPaths.Add(folderPath);
                                            }
                                        }
                                        // TODO: Add /Directory1/Directory2/
                                    }
                                    // TODO: Add /*.xml
                                    // TODO: Add /Directory 1/.../*.xml
                                }
                                // Specific file
                                else
                                {
                                    foreach (var filePath in filePaths)
                                    {
                                        var fileInfo = new FileInfo(filePath);
                                        if (fileInfo.Exists && line == fileInfo.Name)
                                        {
                                            ignoredfilePaths.Add(filePath);
                                        }
                                    }
                                }
                            }
                        }

                    } while (line != null);
                }

                var wixIgnore = new WixIgnore(ignoredfilePaths, ignoredfolderPaths);

                return wixIgnore;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
