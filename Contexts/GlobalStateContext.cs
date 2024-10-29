using System.Text.Json;
using System.Text;

namespace BlazorClient.Contexts
{
    public class GlobalStateContext : IGlobalStateContext
    {
        public event Action OnChange;
        private readonly IServiceProvider _serviceProvider;
        // Define the nested State class
        public class StateData
        {
            public bool Loading { get; set; } 
            public string ApiAddress { get; set; } = string.Empty;
            public string Token { get; set; } = string.Empty;
            public bool IsAuthorized { get; set; }
            public bool ShowLoginModal { get; set; }
        }

        // Property to hold the state
        public StateData State { get; private set; } = new StateData();

        public GlobalStateContext(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            State.ApiAddress = configuration["ApiAddress"];
            _serviceProvider = serviceProvider;
        }

        public void SetApiAddress(string apiAddress)
        {
            State.ApiAddress = apiAddress;
        }

        public void SetAuthorized(bool authorized)
        {
            State.IsAuthorized = authorized;
            NotifyStateChanged();
        }

        public void SetLoading(bool loading)
        {
            State.Loading = loading;
            NotifyStateChanged();
        }

        public void SetShowLoginModal(bool showLoginModal)
        {
            State.ShowLoginModal = showLoginModal;
            NotifyStateChanged();
        }

        public void Login(string token)
        {
            SetAuthorized(true);
            State.Token = token;
        }

        public Task<string> LogoutAsync()
        {
            SetAuthorized(false);
            State.Token = string.Empty;
            return Task.FromResult("userLoggedOut");
        }

        public async Task<string> CheckPasswordAsync(string password)
        {
            SetLoading(true);

            var loginModel = new LoginModel
            {
                Password = password
            };

            // Resolve HttpClient from the service provider
            using var scope = _serviceProvider.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();

            var requestContent = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/authorization/login", requestContent);

            SetLoading(false);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var responseObject = JsonSerializer.Deserialize<LoginResponse>(responseContent);

                if (responseObject != null)
                {
                    Login(responseObject.token);
                }

                return "PasswordOk";
            }

            // Handle error responses here
            var errorResponse = response.Content.ReadAsStringAsync().Result;
            return errorResponse;
        }

        public class LoginModel
        {
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponse
        {
            public string token { get; set; } = string.Empty;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
