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
                                // Extension (e.g. *.pdb)
                                if (line.StartsWith("*."))
                                {
                                    var splitString = line.Split('.');
                                    if (splitString.Length == 2)
                                    {
                                        var extension = splitString[1];

                                        foreach (var filePath in filePaths)
                                        {
                                            if (filePath.EndsWith(extension))
                                            {
                                                ignoredfilePaths.Add(filePath);
                                            }
                                        }
                                    }
                                }
                                // Directory
                                else if (line.StartsWith("/"))
                                {
                                    // e.g. /Directory/ or /Directory_1/Directory_2/
                                    if (line.EndsWith("/"))
                                    {
                                        var directoryString = line.Substring(1, line.Length - 2);
                                        var splitDirectories = directoryString.Split('/');

                                        foreach (var folderPath in folderPaths)
                                        {
                                            var directoryInfo = new DirectoryInfo(folderPath);
                                            if (directoryInfo.Exists)
                                            {
                                                if (directoryInfo.Name == splitDirectories[splitDirectories.Length - 1])
                                                {
                                                    var result = HasMatchingDirectoryStructure(directoryInfo, splitDirectories, splitDirectories.Length - 2, true);
                                                    if (result)
                                                    {
                                                        ignoredfolderPaths.Add(folderPath);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    // e.g. /*.xml or /Directory_1/Directory_2/*.xml
                                    else
                                    {
                                        var directoryString = line;
                                        var splitDirectories = directoryString.Split('/');

                                        var extensionString = splitDirectories[splitDirectories.Length - 1];
                                        
                                        var splitExtensionString = extensionString.Split('.');
                                        if (splitExtensionString.Length == 2)
                                        {
                                            var extension = splitExtensionString[1];
                                            var sourceDirInfo = new DirectoryInfo(sourceDir);
                                            var parentFolder = sourceDirInfo.Name;

                                            if (splitDirectories.Length != 1)
                                            {
                                                if (!string.IsNullOrEmpty(splitDirectories[splitDirectories.Length - 2]))
                                                {
                                                    parentFolder = splitDirectories[splitDirectories.Length - 2];
                                                }
                                            }

                                            foreach (var filePath in filePaths)
                                            {
                                                
                                                if (filePath.EndsWith(extension))
                                                {
                                                    var fileInfo = new FileInfo(filePath);
                                                    if (fileInfo.Exists)
                                                    {
                                                        if (fileInfo.Directory != null && fileInfo.Directory.Name == parentFolder)
                                                        {
                                                            var result = HasMatchingDirectoryStructure(fileInfo.Directory, splitDirectories, splitDirectories.Length - 3, true);
                                                            if (result)
                                                            {
                                                                ignoredfilePaths.Add(filePath);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                // Specific file (e.g. test.txt) or a random string which will not match any file paths
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

        private static bool HasMatchingDirectoryStructure(DirectoryInfo directoryInfo, string[] directories, int index, bool result)
        {
            try
            {
                if (index == -1 || result == false)
                {
                    return result;
                }

                if (directoryInfo.Parent != null && directoryInfo.Parent.Name == directories[index])
                {
                    return HasMatchingDirectoryStructure(directoryInfo.Parent, directories, index - 1, true);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
    }
}
