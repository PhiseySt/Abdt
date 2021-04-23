using System;
using System.Collections.Generic;
using System.Linq;

namespace TextProcessing.StorageData
{
    public static class Storage
    {
        public static Dictionary<int, byte[]> StorageBinaryFiles { get; set; }
        public static Dictionary<int, string> StorageTexts { get; set; }

        public static int GetNextIdStorageBinaryFiles => StorageBinaryFiles.Max(item => item.Key)+1;
        public static int GetNextIdStorageStrings => StorageTexts.Max(item => item.Key) + 1;

        static Storage()
        {
            StorageTexts = new Dictionary<int, string>();
            StorageBinaryFiles= new Dictionary<int, byte[]>();
        }
    }
}
