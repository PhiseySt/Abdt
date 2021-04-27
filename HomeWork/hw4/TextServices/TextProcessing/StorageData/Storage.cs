using System;
using System.Collections.Generic;
using System.Linq;

namespace TextProcessing.StorageData
{
    public static class Storage
    {
        public static Dictionary<DateTime, byte[]> StorageBinaryFiles { get; set; }
        public static Dictionary<int, string> StorageTexts { get; set; }
        public static List<InfoTask> StorageTasks { get; set; }
        public static TaskStorage DataTaskStorage { get; set; }

        public static int GetNextIdStorageStrings => StorageTexts.Max(item => item.Key) + 1;
        public static int GetNextIdStorageTasks => StorageTasks.Max(item => item.Id) + 1;


        static Storage()
        {
            StorageTexts = new Dictionary<int, string>();
            StorageTasks = new List<InfoTask>();
            StorageBinaryFiles = new Dictionary<DateTime, byte[]>();
            DataTaskStorage = new TaskStorage();
        }
    }
}
