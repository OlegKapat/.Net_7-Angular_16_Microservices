using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetProductByBrandnQuery : IRequest<IList<ProductResponse>>
    {
        public string BrandName { get; set; }

        public GetProductByBrandnQuery(string brandname)
        {
            BrandName = brandname;
        }
    }
}
