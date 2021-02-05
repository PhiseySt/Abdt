using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using YandexUploadFiles.DirectoryFileUtils;
using YandexUploadFiles.Loader.Models;
using YandexUploadFiles.Loader.YandexBl;
using YandexUploadFiles.Loader.YandexBL;
using File = YandexUploadFiles.DirectoryFileUtils.File;


namespace YandexUploadFiles
{
    internal class Workflow : IWorkflow
    {
        private const int TreadDelay = 100;
        private List<File> _filesUpload;
        private YandexUploader _yandUploader;
        private string _yandDiskPath;
        private List<FileState> _listFileUploadStates;

        #region FilesWork
        public void FilesWork()
        {

            var localDirectory = GetUserLocalDirectory();
            try
            {
                _filesUpload = DirectoryFileActions.GetDirectoryFileList(localDirectory);
            }
            catch (DirectoryNotFoundException)
            {
                Console.Clear();
                Console.WriteLine($"Похоже, что вы указали некорректную директорию: {localDirectory}. Попробуем еще раз.");
                FilesWork();
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine($"Похоже, какие то проблемы с файлами в указанной директории: {localDirectory}. Попробуем еще раз.");
                FilesWork();
            }
        }

        private static string GetUserLocalDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine("Введите название папки на локальном компьютере, откуда будет производиться загрузка файлов.");
            Console.WriteLine("Файлы будут загружаться и из вложенных папок тоже.");
            Console.WriteLine($"По умолчанию, папкой для загрузки установлена текущая директория: {currentDirectory}");
            Console.WriteLine("Окончание ввода фиксируется нажатием клавиши ENTER.");
            var inputData = Console.ReadLine();
            return string.IsNullOrEmpty(inputData) ? currentDirectory : inputData;
        }

        #endregion
        #region UserWork
        public void UserWork()
        {
            var userToken = GetUserToken();
            var yandexToken = string.IsNullOrEmpty(userToken) ? DefaultSettings.DefaultToken : userToken;
            _yandUploader = new YandexUploader(yandexToken);
            var user = _yandUploader.GetUserInfo();
            if (user?.User != null)
            {
                Console.WriteLine("Коннект к яндекс диску состоялся.");
                Console.WriteLine($"Пользователь  : {user.User.Display_Name} {user.User.Login}");
                Console.WriteLine($"Общее место на диске (Гб) : {MeasureUnit.BytesToGigabytes(user.Total_Space)}");
                Console.WriteLine($"Занятое место на диске (Гб): {MeasureUnit.BytesToGigabytes(user.Used_Space)}");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Произошла ошибка при подключении к диску с указанным токеном.");
                UserWork();
            }
        }

        private string GetUserToken()
        {
            Console.WriteLine("Введите токен яндекс диска. Если вы ничего не ввели, будет использован токен по умолчанию.");
            Console.WriteLine("Окончание ввода фиксируется нажатием клавиши ENTER.");
            var inputData = Console.ReadLine();
            return inputData;
        }

        #endregion
        #region UploadWork
        public void UploadWork()
        {
            _yandDiskPath = GetYandexDirectory();
            if (IsCorrectYandexDirectory(_yandDiskPath))
            {
                try
                {
                    var yandDiskFullPath = string.Concat("disk:/", _yandDiskPath);
                    UploadFile(_filesUpload, yandDiskFullPath).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Что -то пошло не так при загрузке файлов на yandex диск {ex.Message}. ");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Яндекс директория некорректна. {_yandDiskPath}. Попробуйте еще раз.");
                UploadWork();
            }

        }

        private string GetYandexDirectory()
        {
            Console.WriteLine();
            Console.WriteLine($"Введите директорию для загрузки. Если вы ничего не ввели, будет использована директория по умолчанию: {DefaultSettings.DefaultYandexDirectory}");
            Console.WriteLine("Окончание ввода фиксируется нажатием клавиши ENTER.");
            var inputData = Console.ReadLine();
            return string.IsNullOrEmpty(inputData) ? DefaultSettings.DefaultYandexDirectory : inputData;
        }

        private bool IsCorrectYandexDirectory(string path)
        {
            ResourseInfo directoryInfo = null;

            using (var yandHttpClient = new YandexHttpClient(_yandUploader.Token))
            {
                var task = yandHttpClient.GetAsync(DefaultSettings.YandexDiskUrlCheck + path).ContinueWith((requestTask) =>
                 {
                     var response = requestTask.Result;
                     var json = response.Content.ReadAsStringAsync();
                     json.Wait();
                     directoryInfo = JsonConvert.DeserializeObject<ResourseInfo>(json.Result);
                 });
                task.Wait();
            }

            return directoryInfo.Name != null;
        }

        private async Task UploadFile(List<File> files, string pathDisk)
        {
            _listFileUploadStates = new List<FileState>();
            var timer = new System.Timers.Timer(700);

            timer.Elapsed += RefreshConsoleUploadInfo;
            Console.CursorVisible = false;
            Console.Clear();
            Thread.Sleep(TreadDelay);

            Console.WriteLine("Загрузка файлов на яндекс диск" + Environment.NewLine + Environment.NewLine);

            var uploadTasksQuery =
                from url in files
                select ProcessUrlAsync(url, pathDisk);
            timer.Start();
            var uploadTasks = uploadTasksQuery.ToList();

            await Task.WhenAll(uploadTasks);

            timer.Stop();
            Console.ReadKey();
        }

        private async Task<string> ProcessUrlAsync(File file, string pathDisk)
        {
            _listFileUploadStates.Add(new FileState { name = file.Name, state = "Загрузка 0 %" });
            Console.WriteLine($"{file.Name} - Загрузка 0 %");
            var progress = new UploadProgress(UpdateProgress);
            await UploadAsync(file.Path, $"{pathDisk}/{file.Name}", progress);
            return file.Name;
        }

        public async Task UploadAsync(string pathFile, string pathSave, UploadProgress progress, CancellationToken cancelToken = default)
        {
            Stream stream = new FileStream(pathFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var uploadLink = await GetUploadLinkAsync(pathSave);
            var fileName = Path.GetFileName(((FileStream)stream).Name);
            var streamContent = new YandexStreamContent(stream);
            streamContent.ProgressChanged += (bytes, currBytes, totalBytes) => progress.UpdateProgress(currBytes, totalBytes, fileName);
            var content = new MultipartFormDataContent { { streamContent, "file", fileName } };

            using var yandHttpClient = new YandexHttpClient(_yandUploader.Token);
            await yandHttpClient.PostAsync(uploadLink.Href, content, cancelToken);
        }

        private void RefreshConsoleUploadInfo(object sender, System.Timers.ElapsedEventArgs e)
        {
            const string finalReslution = "Загрузка 100 %";
            Console.SetCursorPosition(0, 2);
            var count = _listFileUploadStates.Where(t => t.state == finalReslution).ToList().Count;
            Console.WriteLine($"Всего файлов:{_listFileUploadStates.Count}   Загружено файлов:{count}");
            foreach (var fileState in _listFileUploadStates)
            {
                Console.WriteLine($"{fileState.name} - {fileState.state}");
            }

            Thread.Sleep(TreadDelay);
            Console.SetCursorPosition(0, 2);
        }

        private async Task<UploadResponse> GetUploadLinkAsync(string path)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["path"] = path;
            return JsonConvert.DeserializeObject<UploadResponse>(await GetAsync(query));
        }

        private async Task<string> GetAsync(object param)
        {
            var builder = new UriBuilder(DefaultSettings.YandexDiskUrlUpload) { Query = param.ToString() };
            var url = builder.ToString();
            var yandHttpClient = new YandexHttpClient(_yandUploader.Token);
            var response = await yandHttpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
        private void UpdateProgress(long loadVal, long totalVal, string fileName)
        {
            var percent = Math.Round(loadVal / (double)totalVal * 100, 0);
            var file = _listFileUploadStates?.FirstOrDefault(t => t.name == fileName);

            if (file != null) file.state = $"Загрузка {percent} %";
        }
        #endregion

    }
}
