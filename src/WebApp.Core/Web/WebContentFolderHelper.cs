using Abp.Reflection.Extensions;
using System;
using System.IO;
using System.Linq;

namespace WebApp.Core.Web
{

    /// <summary>
    /// This class is used to find root path of the web project in;
    /// unit tests (to find views) and entity framework core command line commands (to find conn string).
    /// </summary>
    #region
    //public static class WebContentDirectoryFinder
    //{
    //    public static string CalculateContentRootFolder()
    //    {
    //        var coreAssemblyDirectoryPath = Path.GetDirectoryName(AppContext.BaseDirectory);
    //        if (coreAssemblyDirectoryPath == null)
    //        {
    //            throw new Exception("Could not find location of WebApp.Core assembly!");
    //        }

    //        var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
    //        while (!DirectoryContains(directoryInfo.FullName, "WebApp.sln"))
    //        {
    //            if (directoryInfo.Parent == null)
    //            {
    //                throw new Exception("Could not find content root folder!");
    //            }

    //            directoryInfo = directoryInfo.Parent;
    //        }

    //        return Path.Combine(directoryInfo.FullName, $"src{Path.DirectorySeparatorChar}WebApp.Web");
    //    }

    //    private static bool DirectoryContains(string directory, string fileName)
    //    {
    //        return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
    //    }
    #endregion

    public static class WebContentDirectoryFinder
        {
            public static string CalculateContentRootFolder()
            {
                var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(WebAppCoreModule).GetAssembly().Location);
                if (coreAssemblyDirectoryPath == null)
                {
                    throw new Exception("Could not find location of zyGIS.Core assembly!");
                }

                var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
                while (!DirectoryContains(directoryInfo.FullName, "WebApp.sln"))
                {
                    if (directoryInfo.Parent == null)
                    {
                        throw new Exception("Could not find content root folder!");
                    }

                    directoryInfo = directoryInfo.Parent;
                }

                var webMvcFolder = Path.Combine(directoryInfo.FullName, "src", "WebApp.Web.Mvc");
                if (Directory.Exists(webMvcFolder))
                {
                    return webMvcFolder;
                }

                var webHostFolder = Path.Combine(directoryInfo.FullName, "src", "WebApp.Web.Host");
                if (Directory.Exists(webHostFolder))
                {
                    return webHostFolder;
                }

                throw new Exception("Could not find root folder of the web project!");
            }

            private static bool DirectoryContains(string directory, string fileName)
            {
                return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
            }
        }
    }
