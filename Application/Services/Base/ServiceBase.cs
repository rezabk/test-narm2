using Application.Services.Cross;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Base;

public class ServiceBase<TService>
    where TService : class

{
    private readonly IHttpContextAccessor _contextAccessor;

    public ServiceBase(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.CheckArgumentIsNull(nameof(_contextAccessor));
    }

    protected string CurrentUserName => _contextAccessor.GetCurrentUserName();
    protected string CurrentUserNationalCode => _contextAccessor.GetCurrentUserNationalCode();
    protected string CurrentUserFirstName => _contextAccessor.GetCurrentUserFirstName();
    protected string CurrentUserLastName => _contextAccessor.GetCurrentUserLastName();
    protected int CurrentUserId => _contextAccessor.GetCurrentUserIdentityId();
}