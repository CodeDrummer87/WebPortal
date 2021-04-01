using RailroadPortalClassLibrary;
using TCH2_WestSiberianRailroad.Modules.Interfaces;

namespace TCH2_WestSiberianRailroad.Modules.Implementation
{
    public class TCH2_WebClient : AppWebClient, ITCH2_WebClient
    {
        public TCH2_WebClient() : base("https://localhost:44303")
        { }
    }
}
