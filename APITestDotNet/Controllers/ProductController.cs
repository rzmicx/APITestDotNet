using APITestDotNet.Data.Models;
using APITestDotNet.Interfaces;
using APITestDotNet.Services;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

namespace APITestDotNet.Controllers
{
    [Route("api/Product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _itemService;

        public ProductController(IProductService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("addProduct")]
        public IActionResult Add([FromBody] Product request)
        {
            var item = request;
            var idUser = User.FindFirstValue(ClaimTypes.Name);
            var hasil = _itemService.AddPRoduct(idUser, item);
            if (hasil.Result == "ok")
                return Ok(new { message = hasil.Result });

            return BadRequest(new { message = hasil.Result });
        }


        [HttpPost("editProduct")]
        public IActionResult Edit([FromBody] Product request)
        {
            var item = request;
            var idUser = User.FindFirstValue(ClaimTypes.Name);
             
            var hasil = _itemService.EditProduct(idUser, item);
            if (hasil.Result == "ok")
                return Ok(new { message = hasil.Result });

            return BadRequest(new { message = hasil.Result });
        }

        [HttpGet("ViewGrid")]
        public async Task<IActionResult> ViewGrid([FromQuery] string type,string? orderby)
        {
            try
            {
             
                var result = await _itemService.GetProducts(type,orderby);
                 return Ok(result);
                
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(nameof(ViewGrid), ex);
                return BadRequest(new { message = "mohon hubungi tim support" });
            }
        }
    }
}
