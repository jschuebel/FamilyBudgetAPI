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
    public class CategoryXrefController : ControllerBase
    {
        private readonly ILogger<CategoryXrefController> _logger;
        private familyapp.Interface.ICategoryXrefService _catService;

        public CategoryXrefController(ILogger<CategoryXrefController> logger,
                                    familyapp.Interface.ICategoryXrefService catService)
        {
            _logger = logger;
            _catService=catService;
        }

        [HttpGet]
        public IEnumerable<familyapp.Model.CategoryXrefVM> Get()
        {
            var qstr = HttpContext.Request.QueryString.ToString();   //.Query.ToString();
             if (qstr=="") qstr=null;
             var cats = _catService.GetAll(qstr).ToList();
 
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count, X-Paging-PageSize");
            HttpContext.Response.Headers.Add("X-Total-Count", JsonConvert.SerializeObject (_catService.Count));
 
            _logger.LogInformation($"Get method cats count={cats.Count}");
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

        // PUT api/values/5
       // [Authorize]
        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, [FromBody] coreevent.Person item)
//        [HttpPut]
        //public async Task<IActionResult> Put([FromBody] familyapp.Model.ProductVM item)
        public IActionResult Put(int id, [FromBody] familyapp.Model.CategoryVM [] items)
        {
         try 
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException("ModelState must be invalid", nameof(ModelState));
//                if (id != item.Id)
//                    return NotFound("CategoryXref not found"); 
            _logger.LogInformation($"Put method ProductID={id} items count={items.Length}");
                _catService.Update(id, items);
                //return Ok(np);
                return Ok();
            }
            catch (Exception ex)
            {
                var sb = new System.Text.StringBuilder();
                while (ex!=null) {
                    ModelState.AddModelError("CategoryXrefPut", ex.Message);
                    ex=ex.InnerException;
                }
                return BadRequest(ModelState);  
               
            }


        }


    }
}
