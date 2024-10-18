namespace BlazorClient.Contexts
{
    public interface IGlobalStateContext
    {
        GlobalStateContext.StateData State { get; }

        bool SetLoading(bool loading);
        void Login(string userName);
        Task<string> LogoutAsync();

        Task<string> CheckPasswordAsync(string password);
    }
}