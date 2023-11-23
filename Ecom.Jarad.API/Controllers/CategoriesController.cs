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
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork unit;

        public CategoriesController(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        [HttpGet("get-all-categoy")]
        public async Task<IActionResult> get(string type)
        {
            try
            {
                IReadOnlyList<Category> result = await unit.Category.GetAllAsync(type);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(400, ex.Message));
            }

        }
        [HttpPost("add-new-category")]
        public async Task<IActionResult> Add([FromForm] CategoryDTO categoryDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (categoryDTO is not null)
                    {
                        bool result = await unit.Category.AddAsync(categoryDTO);
                        return result ? Ok(new BaseResponse(200)) : BadRequest(new BaseResponse(400));
                    }

                }

                return BadRequest(new BaseResponse(400, "someting went wrong "));

            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(400, ex.Message));

            }
        }
        [HttpPut("update-category")]
        public async Task<IActionResult> update(int id, [FromForm] CategoryUpdateDTO categoryDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (categoryDTO.Name is not null &&
                        categoryDTO.type is not null &&
                        !string.IsNullOrEmpty(id.ToString()))
                    {
                        bool result = await unit.Category.UpdateAsync(id, categoryDTO);
                        return result ? Ok(new BaseResponse(200)) : BadRequest(new BaseResponse(400));
                    }


                }
                return BadRequest(new BaseResponse(400));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(400, ex.Message));
            }
        }
        [HttpDelete("delete-category")]
        public async Task<IActionResult> delete([FromQuery] int id)
        {
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                bool result = await unit.Category.Delete(id);

                return result ? Ok(new BaseResponse(200)) : BadRequest(new BaseResponse(400));
            }
            return BadRequest(new BaseResponse(400, "someting went wrong "));
        }
    }
}
