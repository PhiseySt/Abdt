using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextProcessing.StorageData;

namespace TextProcessing.Controllers
{
    /// <summary>
    /// FindServiceController
    /// </summary>
    [ApiController]
    public class FindServiceController : ControllerBase
    {
        /// <summary>
        /// Заглушка
        /// </summary>
        [Route("api/Get/Text2")]
        [HttpGet]
        public IEnumerable<string> GetListTexts()
        {
            return Storage.StorageTexts.Select(d => d.Value).ToList();
        }
    }
}
