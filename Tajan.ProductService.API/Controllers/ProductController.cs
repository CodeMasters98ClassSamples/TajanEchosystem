using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using Tajan.ProductService.API.Contracts;
using Tajan.ProductService.API.Dtos;
using Tajan.ProductService.API.Entities;
using Tajan.ProductService.API.Settings;
using Tajan.Standard.Presentation.Abstractions;

namespace Tajan.ProductService.API.Controllers;

public class ProductController : CustomController
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

    [HttpGet()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIds([FromQuery] List<int> ids)
    {
        if (ids.Count == 0)
            return BadRequest();

        List<Product> products = new List<Product>();
        return Ok(products);
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
        Product product = new(){};

        //Business Call
        _productService.Add(product);

        return Ok(product.Id);
    }
}
