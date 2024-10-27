namespace Server.Infrastruct.Services.Agent
{
    public interface IHttpAgentService : IRawHttpAgentService
    {
        HttpAgentService InjectToken();
        HttpAgentService InjectToken(Dictionary<string, string> Header);
        new HttpAgentService SetBaseUrl(string baseUrl);

        T Get<T>(string url, Dictionary<string, object>? obj = null, Dictionary<string, string>? Headers = null);
        Task<T> GetAsync<T>(string url, Dictionary<string, object>? obj = null, Dictionary<string, string>? Headers = null);
        T Post<T>(string url, object? obj = null, Dictionary<string, string>? Headers = null);
        Task<T> PostAsync<T>(string url, object? obj = null, Dictionary<string, string>? Headers = null);
    }
}