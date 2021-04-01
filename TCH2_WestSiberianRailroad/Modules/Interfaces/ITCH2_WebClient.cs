using System.Net.Http;

namespace TCH2_WestSiberianRailroad.Modules.Interfaces
{
    public interface ITCH2_WebClient
    {
        string Send<T>(HttpMethod method, string path, T arg);
        string Get(string path, string argsInString);
    }
}
