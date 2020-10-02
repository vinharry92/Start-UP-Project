using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using No_Core_Auth.Data;
using No_Core_Auth.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace No_Core_Auth.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: api/values
        [HttpGet]
        [Authorize(Policy = "RequiredLoggedIn")]
        public IActionResult Get()
        {
            return Ok(_db.productModels.ToList());
        }

        [HttpPost]
        [Route("AddProduct")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult> AddProduct([FromBody] ProductModel formdata)
        {
            var newProduct = new ProductModel
            {
                Name = formdata.Name,
                ImgUrl = formdata.ImgUrl,
                Description = formdata.Description,
                Outofstock = formdata.Outofstock,
                Price = formdata.Price
            };
            await _db.productModels.AddAsync(newProduct);

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("[action]/{id}")]
        [Route("UpdateProduct")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult> UpdateProduct([FromRoute]  int id , [FromBody] ProductModel formdata)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var findProduct = _db.productModels.FirstOrDefault(p => p.Productid == id);
            if(findProduct == null)
            {
                return NotFound();
            }

            //If the product was found
            findProduct.Name = formdata.Name;
            findProduct.ImgUrl = formdata.ImgUrl;
            findProduct.Description = formdata.Description;
            findProduct.Outofstock = formdata.Outofstock;
            findProduct.Price = formdata.Price;

            _db.Entry(findProduct).State = EntityState.Modified;

            await _db.SaveChangesAsync();

            return Ok(new JsonResult("The Product with id  " + id + "is update"));
        }

        [HttpDelete("[action]/{id}")]
        [Route("DeleteProduct")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult> DeleteProduct([FromRoute]  int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findProduct = await _db.productModels.FindAsync(id);
            if(findProduct == null)
            {
                return NotFound();
            }

            _db.productModels.Remove(findProduct);
            await _db.SaveChangesAsync();

            return Ok(new JsonResult("The Product with id  " + id + "is Deleted"));
        }




    }
}
