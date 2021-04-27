using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TextProcessing.StorageData;


namespace TextProcessing.Controllers
{
    /// <summary>
    /// TaskServiceController
    /// </summary>
    [ApiController]
    public class TaskServiceController : Controller
    {
        private static readonly BackgroundWorker WorkerWorkflow = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
        private static Timer _stopstartTimer;
        private static Timer _analyzeTimer;
        /// <summary>
        /// Запуск задачи на обработку файлов
        /// </summary>
        /// <param name="dateStart">Начальная дата запуска задания. Пример: 2021-04-24T08:19:14.488Z</param>
        /// <param name="dateFinish">Конечная дата запуска задания. Пример: 2021-04-26T08:19:14.488Z</param>
        /// <param name="interval">Интервал (мин)</param>
        /// <param name="setWords">Набор слов для поиска</param>
        [Route("api/Get/Workflow")]
        [HttpGet]
        public string StartWorkflow([BindRequired] DateTime dateStart, [BindRequired] DateTime dateFinish, [BindRequired] int interval, [BindRequired][FromQuery] string[] setWords)
        {

            Storage.DataTaskStorage.DateStart = dateStart;
            Storage.DataTaskStorage.DateFinish = dateFinish;
            Storage.DataTaskStorage.Period = interval * 60 * 1000;
            Storage.DataTaskStorage.SetWords = setWords;
            Storage.DataTaskStorage.Id = Storage.StorageTasks.Count > 0 ? Storage.GetNextIdStorageTasks : 1;
            StartTimers(Storage.DataTaskStorage.Period);
            WorkerWorkflow.DoWork += AnalyzeFiles;
            return $"Задача на обработку файлов запущена в {DateTime.Now}";
        }

        private static void StartTimers(int timerInterval)
        {
            // timer start/stop analyze
            _analyzeTimer = new Timer(timerInterval) {AutoReset = true, Enabled = true};
            _analyzeTimer.Elapsed += OnAnalyzeTimedEvent;
            _analyzeTimer.Stop();
            // timer in background for start/stop _analyzeTimer
            _stopstartTimer = new Timer(60 * 1000);
            _stopstartTimer.Elapsed += OnTimedEvent;
            _stopstartTimer.AutoReset = true;
            _stopstartTimer.Enabled = true;
        }

        private static void OnAnalyzeTimedEvent(object source, ElapsedEventArgs e)
        {
            WorkerWorkflow.RunWorkerAsync();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (Storage.DataTaskStorage?.DateStart < DateTime.Now && Storage.DataTaskStorage?.DateFinish > DateTime.Now) _analyzeTimer.Start();
            if (Storage.DataTaskStorage?.DateFinish < DateTime.Now) _analyzeTimer.Stop();
        }

        private static void AnalyzeFiles(object sender, DoWorkEventArgs e)
        {
            var testDelay = 10000; //ввел доп задержку, чтобы интервал для тестирования увеличить
            var period = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(Storage.DataTaskStorage.Period+ testDelay ));
            var filesAfterLastTaskWorkflow = Storage.StorageBinaryFiles.Where(item=> item.Key> period).Select(item=> item.Value);
            var countMatches = 0;
            foreach (var word in Storage.DataTaskStorage.SetWords)
            {
                foreach (var file in filesAfterLastTaskWorkflow)
                {
                    var strFile = System.Text.Encoding.UTF8.GetString(file, 0, file.Length);
                    countMatches += Regex.Matches(strFile, word).Count;
                }
            }

            var messageResult =
                $"Дата запуска задания = {DateTime.Now}. В результате обработки задания с idTask={Storage.DataTaskStorage.Id} в период с {Storage.DataTaskStorage.DateStart}" +
                $" по {Storage.DataTaskStorage.DateFinish} для поискового списка слов {string.Join(" ",Storage.DataTaskStorage.SetWords)} получено количество совпадений = {countMatches}";
            var id = Storage.StorageTasks.Count > 0 ? Storage.DataTaskStorage.Id : 1;
            var data = new InfoTask {Id = id, Info = messageResult};
            Storage.StorageTasks.Add(data);
        }

        /// <summary>
        /// Результаты работ по заданию c указанным id
        /// </summary>
        /// <param name="id">Универсальный идентификатор задания</param>
        [Route("api/Get/GetDataById")]
        [HttpGet]
        public IEnumerable<string> GetDataById([BindRequired] int id)
        {
            return Storage.StorageTasks.Where(item => item.Id == id).Select(d => d.Info).ToList();
        }

    }
}
