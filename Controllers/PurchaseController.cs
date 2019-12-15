using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using familyapp = FamilyBudget.Application;

namespace FamilyBudget.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly ILogger<PurchaseController> _logger;
        private familyapp.Interface.IPurchaseService _purchService;

    //    public PersonController(IPersonService personService)
    //     {
    //         _personService= personService;
    //     }

        public PurchaseController(ILogger<PurchaseController> logger,
                                    familyapp.Interface.IPurchaseService purchService)
        {
            _logger = logger;
            _purchService=purchService;
        }

        [HttpGet]
        public IEnumerable<familyapp.Model.PurchaseVM> Get()
        {
            var qstr = HttpContext.Request.QueryString.ToString();   //.Query.ToString();
            if (qstr=="") qstr=null;
            var prods = _purchService.GetAll(qstr).ToList();
 
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count, X-Paging-PageSize");
            HttpContext.Response.Headers.Add("X-Total-Count", JsonConvert.SerializeObject (_purchService.Count));
 
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
