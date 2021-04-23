using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TextProcessing.Controllers
{
    public class TaskServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
