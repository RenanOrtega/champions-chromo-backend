using System.Security.Claims;
using ChampionsChromo.Application.Commands.Interfaces;
using MediatR;

namespace ChampionsChromo.Api.Middlewares
{
    public class UserIdentityBehaviorMiddleware<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IUserCommand userCommand)
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("User not authenticated");
                }

                userCommand.UserId = userId;
            }

            return await next();
        }
    }
}
