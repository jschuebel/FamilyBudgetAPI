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
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private familyapp.Interface.IProductService _prodService;

    //    public PersonController(IPersonService personService)
    //     {
    //         _personService= personService;
    //     }

        public ProductController(ILogger<ProductController> logger,
                                    familyapp.Interface.IProductService prodService)
        {
            _logger = logger;
            _prodService=prodService;
        }

        [HttpGet]
        public IEnumerable<familyapp.Model.ProductVM> Get()
        {
            var qstr = HttpContext.Request.QueryString.ToString();   //.Query.ToString();
             if (qstr=="") qstr=null;
             var prods = _prodService.GetAll(qstr).ToList();
 
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count, X-Paging-PageSize");
            HttpContext.Response.Headers.Add("X-Total-Count", JsonConvert.SerializeObject (_prodService.Count));
 
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

       // POST api/values
        [HttpPost]
        //[Authorize]
        public ActionResult<familyapp.Model.ProductVM> Post([FromBody] familyapp.Model.ProductVM value)
        {
            try
            {
                //string authorization = Request.Headers["Authorization"]; 
                if (!ModelState.IsValid)
                    throw new ArgumentException("ModelState must be invalid", nameof(ModelState));
                var np = _prodService.Create(value);
                return Ok(np);
            }
            catch (Exception ex)
            {
                var sb = new System.Text.StringBuilder();
                while (ex!=null) {
                    ModelState.AddModelError("Product:Post", ex.Message);
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
        public IActionResult Put([FromBody] familyapp.Model.ProductVM item)
        {
         try 
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException("ModelState must be invalid", nameof(ModelState));
//                if (id != item.Id)
//                    return NotFound("Person not found"); 
                var np = _prodService.Update(item);
                return Ok(np);
            }
            catch (Exception ex)
            {
                var sb = new System.Text.StringBuilder();
                while (ex!=null) {
                    ModelState.AddModelError("Product:Put", ex.Message);
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
                if (! await _prodService.Delete(id))
                    return NotFound("Person not found");
                return Ok();
            }
            catch(Exception ex) {

                var sb = new System.Text.StringBuilder();
                while (ex!=null) {
                    ModelState.AddModelError("Product:Get", ex.Message);
                    ex=ex.InnerException;
                }
              return BadRequest(ModelState);  
            }
        }

    }
}
