using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace CacheLocalDisk
{
    class Program
    {
        private static DateTime _lastStart = DateTime.Now;
        private static bool _isFirstStart = true;
        private static string DiskName = "D:\\";
        // Уровень вложенности папок
        private const byte NestingLevel = 5;
        private const string FileExtension = "dll";
        private static ConcurrentBag<string> _listAllFiles = new ConcurrentBag<string>();
        // Обновление кэша с диска раз в 5 минут
        private const int ReadTimerInterval = 300000;
        // Вывод на экран раз в 2 минуты
        private const int WriteTimerInterval = 120000;
        private static Timer _writeTimer;



        static void GetAllFiles(string path, ConcurrentBag<string> files)
        {
            try
            {
                Directory.GetFiles(path).Where(item => DirectoryLevel(item) <= NestingLevel).ToList()
                    .ForEach(files.Add);

                Directory.GetDirectories(path).Where(item => DirectoryLevel(item) <= NestingLevel).ToList()
                    .ForEach(f => GetAllFiles(f, files));

            }
            catch (UnauthorizedAccessException ex)
            {
                //Console.WriteLine(ex);
            }
            finally
            {
                _lastStart = DateTime.Now;
            }
        }
        static void PrintFilesSpecExtension(string nameExtension)
        {
            var printStringBuilder = new StringBuilder($"Актуальный список файлов на дату {DateTime.Now} с расширением {nameExtension}" + Environment.NewLine);
            foreach (var fileName in _listAllFiles.Where(item => item.Contains(nameExtension)).OrderBy(file=>file))
            {
                printStringBuilder.Append(fileName);
                printStringBuilder.AppendLine();
            }
            Console.WriteLine(printStringBuilder.ToString());
        }

        private static int DirectoryLevel(string currentDirectory) => currentDirectory.Split("\\").Length - 1;

        private static void SetWriteTimer()
        {
            _writeTimer = new Timer(WriteTimerInterval);
            _writeTimer.Elapsed += OnTimedEvent;
            _writeTimer.AutoReset = true;
            _writeTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Workflow();
        }

        static void Main(string[] args)
        {
            Workflow();
            _isFirstStart = false;
            SetWriteTimer();
            Console.ReadKey();
        }

        private static void Workflow()
        {
            Console.Clear();
            if (IsNeedStartReading() || _isFirstStart)
            {
                _listAllFiles.Clear();
                GetAllFiles(DiskName, _listAllFiles);
            }
            PrintFilesSpecExtension(FileExtension);
        }

        private static bool IsNeedStartReading() => (DateTime.Now - _lastStart).TotalMinutes > ReadTimerInterval/60000;
    }

}
