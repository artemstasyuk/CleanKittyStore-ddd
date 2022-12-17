using CatStore.Application.Common.Interfaces.Cache;
using CatStore.Application.Common.Interfaces.Persistence;
using CatStore.Domain.BasketAggregate;
using CatStore.Domain.Common.Errors;
using MediatR;
using ErrorOr;

namespace CatStore.Application.MediaR.Baskets.Commands.RemoveItem;

public class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, ErrorOr<Basket>>
{
    private readonly ICacheService _cacheService;
    private readonly IUserRepository _userRepository;

    public RemoveItemCommandHandler(ICacheService cacheService, IUserRepository userRepository)
    {
        _cacheService = cacheService;
        _userRepository = userRepository;
    }
    
    public async Task<ErrorOr<Basket>> Handle(RemoveItemCommand command, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByIdAsync(command.UserId) is not { } user)
            return Errors.User.NotFound;

        var basket = await _cacheService.GetRedisCache<Basket>(user.Id.ToString());
        
        basket.RemoveItem(command.CatId);
        
        await _cacheService.SetRedisCache(user.Id.ToString(), basket);

        return basket;
        
    }
}