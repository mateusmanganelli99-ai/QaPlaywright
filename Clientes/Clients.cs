using Microsoft.Playwright;

namespace QaPlaywright.API;

public class ApiClient
{
    private readonly IAPIRequestContext _request;

    public ApiClient(IAPIRequestContext request)
    {
        _request = request;
    }

    public async Task<IAPIResponse> GetAsync(string url)
    {
        return await _request.GetAsync(url);
    }

    public async Task<IAPIResponse> PostAsync(string url, object body)
    {
        return await _request.PostAsync(url, new()
        {
            DataObject = body
        });
    }
    
}