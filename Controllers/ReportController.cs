using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using familyapp = FamilyBudget.Application;

using Microsoft.AspNetCore.Authorization;

namespace FamilyBudget.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private familyapp.Interface.IReportService _rptService;

    //    public PersonController(IPersonService personService)
    //     {
    //         _personService= personService;
    //     }

        public ReportController(ILogger<ReportController> logger,
                                    familyapp.Interface.IReportService rptService)
        {
            _logger = logger;
            _rptService=rptService;
        }

        [HttpGet]
        public IEnumerable<familyapp.Model.Report1> Get()
        {
            var qstr = HttpContext.Request.QueryString.ToString();   //.Query.ToString();
             if (qstr=="") qstr=null;
             var prods = _rptService.GetAll(qstr).ToList();
 
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count, X-Paging-PageSize");
            HttpContext.Response.Headers.Add("X-Total-Count", JsonConvert.SerializeObject (_rptService.Count));
 
            return prods;
            // var rng = new Random();
            // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            // {
            //     Date = DateTime.Now.AddDays(index),
            //     TemperatureC = rng.Next(-20, 55),
            //     Summary = Summaries[rng.Next(Summaries.Length)]
            // })
            // .ToArray();
        }
    }
}
