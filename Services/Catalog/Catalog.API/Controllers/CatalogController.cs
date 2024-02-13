using System.Net.Mime;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    public class CatalogController : ApiController
    {
        private readonly IMediator _madiator;
        public ILogger<CatalogController> _logger;

        public CatalogController(IMediator madiator, ILogger<CatalogController> logger)
        {
            _logger = logger;
            _madiator = madiator;
        }

        [HttpGet]
        [Route("[action]/{id}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponse>> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _madiator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{productname}", Name = "GetProductByProductName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductByProductName(
            string productname
        )
        {
            var query = new GetProductByNameQuery(productname);
            var result = await _madiator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<ProductResponse>>> GetAllProducts(
            [FromQuery] CatalogSpecParams catalogSpecParams
        )
        {
            var query = new GetAllProductsQuery(catalogSpecParams);
            var result = await _madiator.Send(query);
              _logger.LogInformation("All products retrieved");
            if (result.Count == 0)
                return NotFound("Could not find products");
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<BrandResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<BrandResponse>>> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _madiator.Send(query);
            if (result.Count == 0)
                return NotFound("Could not find brands");
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<TypesResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<TypesResponse>>> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _madiator.Send(query);
            if (result.Count == 0)
                return NotFound("Could not find types");
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{brand}", Name = "GetProductByBrandName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductByBrandName(string brand)
        {
            var query = new GetProductByBrandnQuery(brand);
            var result = await _madiator.Send(query);
            if (result.Count == 0)
                return NotFound("Could not find product by name");
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ProductResponse>> CreateProduct(
            [FromBody] CreateProductCommand productCommand
        )
        {
            var result = await _madiator.Send(productCommand);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(
            [FromBody] UpdateProductCommand productCommand
        )
        {
            var result = await _madiator.Send(productCommand);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var query = new DeleteProductByIdQuery(id);
            return Ok(await _madiator.Send(query));
        }
    }
}
