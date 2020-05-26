using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISession = GInterface23;
using ClientConfig = GClass266;
using Comfort.Common;
using EFT;

namespace EmuTarkov.SinglePlayer.Utils
{
    class Config
    {
        public static ISession BackEndSession => Singleton<ClientApplication>.Instance.GetClientBackEndSession();

        public static string BackendUrl => ClientConfig.Config.BackendUrl;
    }
}
