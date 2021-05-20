using FindService.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TaskService.Entities.Models;
using TaskService.Services.Interfaces;

namespace TaskService.Services.BackgroundServices
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly IFindClient _iFindClient;
        private readonly ITaskService _iTaskService;
        private readonly ITextTaskService _iTextTaskService;

        private readonly ILogger<BackgroundWorker> _logger;
        private Timer _timer;
        TimeSpan ThreeMinutesInterval = TimeSpan.FromMinutes(3);


        TaskModel CurrentTaskModel = null;
        public BackgroundWorker(IFindClient iFindClient, ITaskService iTaskService, ITextTaskService iTextTaskService, ILogger<BackgroundWorker> logger)
        {
            _iFindClient = iFindClient;
            _iTaskService = iTaskService;
            _iTextTaskService = iTextTaskService;
            _logger = logger;

            if (CurrentTaskModel is null)
            {
                CreateTaskAsync().Wait();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var timeSpan = CurrentTaskModel.TaskEndTime.Subtract(CurrentTaskModel.TaskStartTime);
                await Task.Delay(timeSpan, stoppingToken);
            }
        }
        public override Task StartAsync(CancellationToken stoppingToken)
        {
            var period = CurrentTaskModel?.TaskInterval == 0 ? TimeSpan.FromMinutes(CurrentTaskModel.TaskInterval) : ThreeMinutesInterval;
            _timer = new Timer(Workflow, null, TimeSpan.Zero, period);

            return Task.CompletedTask;
        }
        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public override void Dispose()
        {
            _timer?.Dispose();
            CurrentTaskModel = null;
        }
    
        private void Workflow(object state)
        {
            try
            {
              if (CurrentTaskModel is not null)
                {
                    var allTexts = _iFindClient.GetAllTextsAsync().Result;
                    var allNewTexts = allTexts.Where(x => DateTime.UtcNow.Subtract( x.CreatedDate) < ThreeMinutesInterval);

                    if (allNewTexts is not null && allNewTexts.Count() > 0)
                    {
                  
                        string[] words = CurrentTaskModel?.TaskFindWordsModels.Select(x => x.FindWord).ToArray();

                        foreach (var item in allNewTexts)
                        {
                            var infoAboutMatch = _iFindClient.GetWordsByMaskAsync(item.Id, words).Result;

                            var resultDigits = Regex.Match(infoAboutMatch, @"\d+").Value;
                            var textTaskModel = new TextTaskModel
                            {
                                TaskId = CurrentTaskModel.Id,
                                TextId = item.Id,
                                FindindWordsCount = Convert.ToInt32(resultDigits)
                            };

                            _iTextTaskService.CreateTextTaskAsync(textTaskModel).Wait();
                        }
                    }
                }
                else
                {
                    CreateTaskAsync().Wait();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private async Task CreateTaskAsync()
        {
            var firstTestWord = "uuu";
            var secondTestWord = "Quod";
            TaskFindWordsModel[] wordsList = { new TaskFindWordsModel { FindWord = firstTestWord }, new TaskFindWordsModel { FindWord = secondTestWord } };
            CurrentTaskModel = await AddNewTask(DateTime.Now, DateTime.Now.AddMinutes(9), 3, wordsList);
        }

        public async Task<TaskModel> AddNewTask(DateTime startDate, DateTime endDate, int interval, TaskFindWordsModel[] words)
        {
            var taskModel = new TaskModel
            {
                Id = new Guid(),
                TaskStartTime = startDate,
                TaskEndTime = endDate,
                TaskInterval = interval,
                TaskFindWordsModels = words
            };

            return await _iTaskService.CreateTaskAsync(taskModel);
        }

        public async Task<string> FindWords(string word)
        {
            if (!string.IsNullOrEmpty(word))
            {
                return await _iFindClient.IsExistWordAsync(word);
            }
            return null;
        }
    }
}
