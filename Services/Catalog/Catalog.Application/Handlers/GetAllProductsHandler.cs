using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        public ILogger<GetAllProductsHandler> _logger;
        public GetAllProductsHandler(IProductRepository productRepository, ILogger<GetAllProductsHandler> logger)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts(request.CatalogSpecParams);
            var productRepositoryList = ProductMapper.Mapper.Map<Pagination<ProductResponse>>(productList);
             _logger.LogDebug("Received Product List.Total Count: {productList}", productRepositoryList.Count);
            return productRepositoryList;
        }
    }
}