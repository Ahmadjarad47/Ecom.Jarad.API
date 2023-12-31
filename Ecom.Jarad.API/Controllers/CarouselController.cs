using AutoMapper;
using Ecom.Jarad.API.Errors;
using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Jarad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarouselController : ControllerBase
    {
        private readonly IUnitOfWork unit;


        public CarouselController(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        [HttpGet("get-all-carousel")]
        public async Task<IActionResult> getcarousel()

        => Ok(await unit.Carousel.GetAllAsync());

        [HttpPost("add-new-carousel")]
        public async Task<IActionResult> addcarousel([FromForm] CarouselDTO carouselDTO)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    // add to database
                    bool response = await unit.Carousel.AddAsync(carouselDTO);
                    if (response)
                    {
                        return Ok(new BaseResponse(200));
                    }


                }
                return BadRequest(new BaseResponse(400));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(400, ex.Message));
            }


        }
        [HttpPut("update-by-id/{id}")]
        public async Task<IActionResult> update(int id, [FromForm] CarouselDTO carouselDTO)
        {
            try
            {
                //check if the request is valid
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(id.ToString()))
                    {
                        return BadRequest(new BaseResponse(400));
                    }

                    //get carousel by id
                    Carousel findId = await unit.Carousel.GetAsync(id);

                    if (findId is null)
                    {
                        return BadRequest(new BaseResponse(400));
                    }
                    //get carousel by id and update the value
                    bool result = await unit.Carousel.UpdateAsync(id, carouselDTO);

                    if (result)
                    {
                        return Ok(new BaseResponse(200));
                    }

                }

                return BadRequest(new BaseResponse(400));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(400, ex.Message));
            }
        }
        [HttpDelete("delete-carousel-by-id/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                //get carousel by id
                Carousel findId = await unit.Carousel.GetAsync(id);

                if (findId is null)
                {
                    return BadRequest(new BaseResponse(400));
                }

                bool result = await unit.Carousel.DeleteAsync(id);


                if (result)
                {
                    return Ok(new BaseResponse(200));
                }

                return BadRequest(new BaseResponse(400));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(400, ex.Message));
            }
        }
    }
}
