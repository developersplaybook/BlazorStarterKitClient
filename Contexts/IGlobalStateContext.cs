namespace BlazorClient.Contexts
{
    public interface IGlobalStateContext
    {
        event Action OnChange;
        GlobalStateContext.StateData State { get; }

        void SetLoading(bool loading);

        void SetShowLoginModal(bool loading);

        void Login(string userName);
        
        Task<string> LogoutAsync();

        Task<string> CheckPasswordAsync(string password);
    }
}