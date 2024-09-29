using BlazorClient.Contexts;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly IGlobalStateContext _globalStateContext;

    public AuthorizationMessageHandler(IGlobalStateContext globalStateContext)
    {
        _globalStateContext = globalStateContext;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _globalStateContext.State?.Token;
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("*"));

        return await base.SendAsync(request, cancellationToken);
    }
}
