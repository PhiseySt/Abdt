using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YandexUploadFiles.DirectoryFileUtils
{
    internal class DirectoryFileActions
    {
        internal static List<File> GetDirectoryFileList(string localDirectory)
        {
            var allfiles = Directory.GetFiles(localDirectory, "*.*", SearchOption.AllDirectories);
            var listFiles = new List<File>();
                    listFiles.AddRange(allfiles.Select(filename => new File
                        {Name = new DirectoryInfo(filename).Name, Path = filename}));

            return listFiles;
        }
    }
}
