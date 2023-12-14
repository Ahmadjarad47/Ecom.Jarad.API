using Ecom.Jarad.API.Errors;
using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Jarad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly IUnitOfWork unit;

        public SubCategoryController(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        [HttpGet("get-all-subcategory")]
        public async Task<IActionResult> get()
        {
            try
            {
                return Ok(await unit.SubCategory.GetAllAsync());
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(400, ex.Message));
            }
        }






        [HttpPost("add-subcategory")]
        public async Task<IActionResult> add(SubCategoryDTO subCategoryDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = await unit.SubCategory.AddAsync(subCategoryDTO);

                    return Ok(new BaseResponse(200, result));
                }
                return BadRequest(new BaseResponse(400, "something went wrong"));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(400, ex.Message));
            }
        }









        [HttpDelete("delete-sub-category")]
        public async Task<ActionResult> remove([FromQuery] int? id)
        {
            if (id is not null)
            {
                bool result = await unit.SubCategory.Remove(id);
                return result ? Ok(new BaseResponse(200, "item is removed")) : BadRequest(new BaseResponse(400, "something went wrong "));
            }
            return BadRequest(new BaseResponse(400, "id is null"));
        }
    }
}
