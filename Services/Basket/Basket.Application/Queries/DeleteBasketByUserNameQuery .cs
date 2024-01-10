using MediatR;

namespace Basket.Application.Queries
{
    public class DeleteBasketByUserNameQuery : IRequest<bool>
    {
        public string UserName { get; set; }

        public DeleteBasketByUserNameQuery(string userName)
        {
            UserName = userName;
        }
    }
}
