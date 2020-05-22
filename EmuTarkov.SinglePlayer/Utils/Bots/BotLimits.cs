using EmuTarkov.Common.Utils.HTTP;
using EmuTarkov.SinglePlayer.Utils.Reflection;
using ISession = GInterface23;
using ClientConfig = GClass266;
using UnityEngine;
using Newtonsoft.Json;

namespace EmuTarkov.SinglePlayer.Utils.Bots
{
    public class BotLimits
    {
        public static BotLimitsData Data;

        public BotLimits()
        {
            Data = null;
        }

        public static void RequestData()
        {
            ISession session = ClientAppUtils.GetBackendSession();
            string backendUrl = ClientConfig.Config.BackendUrl;

            if (Data != null || session == null)
            {
                return;
            }

            string json = new Request(session.GetPhpSessionId(), backendUrl).GetJson("/client/game/bots/limits");
            Data = JsonConvert.DeserializeObject<BotLimitsData>(json);

            if (Data == null)
            {
                Debug.LogError("EmuTarkov.SinglePlayer: Received bot limits data is NULL, using fallback");
                Data = new BotLimitsData();
            }
            else
            {
                Debug.LogError("EmuTarkov.SinglePlayer: Sucessfully received bot limits data");
            }
        }
    }

    public class BotLimitsData
    {
        public int assault;
        public int cursedAssault;
        public int marksman;
        public int pmcBot;
        public int bossBully;
        public int bossGluhar;
        public int bossKilla;
        public int bossKojaniy;
        public int bossStormtrooper;
        public int bossTest;
        public int followerBully;
        public int followerGluharAssault;
        public int followerGluharScout;
        public int followerGluharSecurity;
        public int followerGluharSnipe;
        public int followerKojaniy;
        public int followerStormtrooper;
        public int followerTest;

        public BotLimitsData()
        {
            assault = 30;
            cursedAssault = 30;
            marksman = 30;
            pmcBot = 30;
            bossBully = 30;
            bossGluhar = 30;
            bossKilla = 30;
            bossKojaniy = 30;
            bossStormtrooper = 30;
            bossTest = 30;
            followerBully = 30;
            followerGluharAssault = 30;
            followerGluharScout = 30;
            followerGluharSecurity = 30;
            followerGluharSnipe = 30;
            followerKojaniy = 30;
            followerStormtrooper = 30;
            followerTest = 30;
        }
    }
}
