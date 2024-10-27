namespace Server.Infrastruct.Services.Agent
{
    public interface IRawHttpAgentService
    {
        T? GetRaw<T>(string url, Dictionary<string, object>? obj = null, Dictionary<string, string>? Headers = null);
        Task<T?> GetRawAsync<T>(string url, Dictionary<string, object>? obj = null, Dictionary<string, string>? Headers = null);
        T? PostRaw<T>(string url, object? obj = null, Dictionary<string, string>? Headers = null);
        Task<T?> PostRawAsync<T>(string url, object? obj = null, Dictionary<string, string>? Headers = null);
        IRawHttpAgentService SetBaseUrl(string baseUrl);

        Task<T?> PostFormDataAsync<T>(string url, Dictionary<string, object>? parameters = null, Dictionary<string, string>? Headers = null) where T : class?;

        T? PostFormData<T>(string url, Dictionary<string, object>? parameters = null, Dictionary<string, string>? Headers = null) where T : class?;
        void SetTimeout(int seconds);
        Task<string?> PostRawAsync(string url, object? obj = null, Dictionary<string, string>? Headers = null);
        Task<string?> GetRawAsync(string url, Dictionary<string, object>? obj = null, Dictionary<string, string>? Headers = null);
    }
}