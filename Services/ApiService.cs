using Microsoft.JSInterop;
namespace BlazorClient.Services;
public class ApiService
{
    private readonly IJSRuntime _jsRuntime;

    public ApiService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<T> GetHelperAsync<T>(string url)
    {
        return await _jsRuntime.InvokeAsync<T>("apiHelper.getHelper", url);
    }
}

