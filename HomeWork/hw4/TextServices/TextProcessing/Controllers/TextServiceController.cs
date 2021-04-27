using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TextProcessing.StorageData;

namespace TextProcessing.Controllers
{
    /// <summary>
    /// TextServiceController
    /// </summary>
    [ApiController]
    public class TextServiceController : ControllerBase
    {
        /// <summary>
        /// Получить список всех текстов из локального хранилища
        /// </summary>
        [Route("api/Get/Text")]
        [HttpGet]
        public IEnumerable<string> GetListTexts()
        {
            return Storage.StorageTexts.Select(d => d.Value).ToList();
        }

        /// <summary>
        /// Получить текст из локального хранилища по id
        /// </summary>
        /// <param name="id">Универсальный идентификатор текста</param>
        [Route("api/Get/Text/{id}")]
        [HttpGet]
        public string GetTextById([BindRequired] int id)
        {
            return Storage.StorageTexts.Where(item => item.Key == id).Select(d => d.Value).FirstOrDefault();
        }

        /// <summary>
        /// Сохранить текст в локальное хранилище
        /// </summary>
        /// <param name="info">Текст для сохранения в локальном хранилище</param>
        [Route("api/Post/Text")]
        [HttpPost]
        public void PostText([BindRequired] string info)
        {
            var id = Storage.StorageTexts.Count > 0 ? Storage.GetNextIdStorageStrings : 1;
            Storage.StorageTexts.Add(id, info);
        }

        /// <summary>
        /// Загрузить текстовый файл с компьютера в локальное хранилище
        /// </summary>
        /// <param name="file">Файл для загрузки в хранилище</param>
        [HttpPost]
        [Route("api/Upload/File")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile([BindRequired] IFormFile file, CancellationToken cancellationToken)
        {
            if (file.Length > 0)
            {
                var pathLocalCopy = await WriteFileLocalCopy(file);
                await WriteFileToStorage(pathLocalCopy);
            }
            else
            {
                return BadRequest(new { message = "Неудачная загрузка файла" });
            }

            return Ok();
        }

        /// <summary>
        /// Загрузить текстовый файл по url ссылке в локальное хранилище
        /// Пример текстового файла: https://filesamples.com/samples/document/txt/sample3.txt
        /// </summary>
        /// <param name="file">Ccылка на url для загрузки файла в хранилище</param>
        [HttpPost]
        [Route("api/Upload/Url")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFileUrl([BindRequired] string url, CancellationToken cancellationToken)
        {
            if (url.Length > 0)
            {
                using var httpClient = new HttpClient();
                var stream = await httpClient.GetStreamAsync(url, cancellationToken);
                using var sr = new StreamReader(stream);
                var fileData = sr.ReadToEnd();
                var id = Storage.StorageTexts.Count > 0 ? Storage.GetNextIdStorageStrings : 1;
                Storage.StorageTexts.Add(id, fileData);
            }
            else
            {
                return BadRequest(new { message = "Неудачная загрузка файла" });
            }
            return Ok();
        }



        private async Task<string> WriteFileLocalCopy(IFormFile file)
        {
            bool isSaveSuccess;
            var path = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var fileName = DateTime.Now.Ticks + extension;

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
                 fileName);
                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                isSaveSuccess = true;
            }
            catch (Exception e)
            {
                isSaveSuccess = false;
            }

            return path;
        }

        private async Task<bool> WriteFileToStorage(string fileName)
        {
            bool isSaveSuccess = false;
            try
            {
                await using (FileStream fstream = new FileStream(fileName, FileMode.Open))
                {
                    byte[] array = new byte[fstream.Length];

                    fstream.Read(array, 0, array.Length);
                    Storage.StorageBinaryFiles.Add(DateTime.Now, array);
                }

                isSaveSuccess = true;
            }
            catch (Exception e)
            {
                //log error
            }

            return isSaveSuccess;
        }

    }
}

