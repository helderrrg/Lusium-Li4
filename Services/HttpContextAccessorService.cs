using Microsoft.AspNetCore.Http;

public class HttpContextAccessorService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextAccessorService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public HttpContext HttpContext => _httpContextAccessor.HttpContext;
}
