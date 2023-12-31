using Ecom.Jarad.API.Errors;
using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Jarad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unit;

        public ProductController(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        [HttpGet("get-arrivals-products")]
        public async Task<IActionResult> get()
        => Ok(await unit.Products.GetarrivalsAsync());


        [HttpGet("get-trending-products")]
        public async Task<IActionResult> get2()
        => Ok(await unit.Products.GettrendingAsync());


        [HttpGet("get-toprated-products")]
        public async Task<IActionResult> get3()
        => Ok(await unit.Products.GettTopRatedAsync());


        [HttpPost("add-item-in-products")]
        public async Task<IActionResult> add([FromForm] ProductsDTO products)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = await unit.Products.AddAsync(products);
                    return result ? Ok(new BaseResponse(200)) : BadRequest(new BaseResponse(404));
                }
                return BadRequest(new BaseResponse(404));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(404, ex.Message));

            }
        }


        [HttpPut("update-item-in-products")]
        public async Task<IActionResult> update([Required] int? id, [FromForm] ProductsDTO products)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = await unit.Products.UpdateAsync(id, products);
                    return Ok(new BaseResponse(200));
                }
                return BadRequest(new BaseResponse(404));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(404, ex.Message));

            }
        }
        [HttpDelete("delete-item-products")]
        public async Task<IActionResult> Delete(int Id)
        {
            await unit.Products.DeleteAsync(Id);
            return Ok(new BaseResponse(200));
        }
    }
}
