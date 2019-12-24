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


       // POST api/values
        [HttpPost]
        //[Authorize]
        public ActionResult<familyapp.Model.CategoryVM> Post([FromBody] familyapp.Model.CategoryVM value)
        {
            try
            {
                //string authorization = Request.Headers["Authorization"]; 
                if (!ModelState.IsValid)
                    throw new ArgumentException("ModelState must be invalid", nameof(ModelState));
                var np = _catService.Create(value);
                return Ok(np);
            }
            catch (Exception ex)
            {
                var sb = new System.Text.StringBuilder();
                while (ex!=null) {
                    ModelState.AddModelError("Category:Post", ex.Message);
                    ex=ex.InnerException;
                }
                return BadRequest(ModelState);  
               
            }
        }

        // PUT api/values/5
       // [Authorize]
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, [FromBody] coreevent.Person item)
        [HttpPut]
        //public async Task<IActionResult> Put([FromBody] familyapp.Model.ProductVM item)
        public IActionResult Put([FromBody] familyapp.Model.CategoryVM item)
        {
         try 
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException("ModelState must be invalid", nameof(ModelState));
//                if (id != item.Id)
//                    return NotFound("Person not found"); 
                var np = _catService.Update(item);
                return Ok(np);
            }
            catch (Exception ex)
            {
                var sb = new System.Text.StringBuilder();
                while (ex!=null) {
                    ModelState.AddModelError("Category:Put", ex.Message);
                    ex=ex.InnerException;
                }
                return BadRequest(ModelState);  
               
            }


        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
     //   [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try {
                if (! await _catService.Delete(id))
                    return NotFound("Category not found");
                return Ok();
            }
            catch(Exception ex) {

                var sb = new System.Text.StringBuilder();
                while (ex!=null) {
                    ModelState.AddModelError("Category:Get", ex.Message);
                    ex=ex.InnerException;
                }
              return BadRequest(ModelState);  
            }
        }


    }
}
