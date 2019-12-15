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
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private familyapp.Interface.ICategoryService _catService;

    //    public PersonController(IPersonService personService)
    //     {
    //         _personService= personService;
    //     }

        public CategoryController(ILogger<CategoryController> logger,
                                    familyapp.Interface.ICategoryService catService)
        {
            _logger = logger;
            _catService=catService;
        }

        [HttpGet]
        public IEnumerable<familyapp.Model.CategoryVM> Get()
        {
            var qstr = HttpContext.Request.QueryString.ToString();   //.Query.ToString();
             if (qstr=="") qstr=null;
             var cats = _catService.GetAll(qstr).ToList();
 
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count, X-Paging-PageSize");
            HttpContext.Response.Headers.Add("X-Total-Count", JsonConvert.SerializeObject (_catService.Count));
 
            return cats;
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
