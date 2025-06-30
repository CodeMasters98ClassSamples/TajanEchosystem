using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Runtime;
using Tajan.ProductService.API.Contracts;
using Tajan.ProductService.API.Dtos;
using Tajan.ProductService.API.Entities;
using Tajan.ProductService.API.Settings;

namespace Tajan.ProductService.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController : ControllerBase
{
    private MySettings _settings;
    private readonly IProductService _productService;

    public ProductController(
        IProductService productService,
        IOptionsMonitor<MySettings> settings)
    {
        _settings = settings.CurrentValue;
        _productService = productService;
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] ProductDto dto)
    {
        if (string.IsNullOrEmpty(dto.Name))
            return BadRequest();
        
        //dto to object
        //1. Install Package
        //2. Mapping by developer
        Product product = new()
        {
            Name = dto.Name
        };

        //Business Call
        _productService.Add(product);

        return Ok(product.Id);
    }
}
