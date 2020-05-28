using UnityEngine;
using Comfort.Common;
using EFT;
using ISession = GInterface24;

namespace EmuTarkov.SinglePlayer.Utils.Reflection
{
    internal static class ClientAppUtils
    {
        public static ClientApplication GetClientApp()
        {
            ClientApplication clientApp = Singleton<ClientApplication>.Instance;

            if (clientApp == null)
            {
                return null;
            }

            return clientApp;
        }

        public static MainApplication GetMainApp()
        {
            ClientApplication clientApp = GetClientApp();

            if (clientApp == null)
            {
                return null;
            }

            return clientApp as MainApplication;
        }

        public static ISession GetBackendSession()
        {
            ISession session = GetClientApp()?.GetClientBackEndSession();

            if (session == null)
            {
                return null;
            }

            return session;
        }

        public static string GetSessionId()
        {
            ISession backend = GetBackendSession();
            return (backend?.Profile == null) ? null : backend.GetPhpSessionId();
        }
    }
}