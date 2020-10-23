using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Smallpox.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpPost("saveEnterpriseData")]
        public void SaveEnterpriseData(string enterpriseName, string manager, string address, string email, string phone)
        {
            Console.WriteLine();
        }
    }
}