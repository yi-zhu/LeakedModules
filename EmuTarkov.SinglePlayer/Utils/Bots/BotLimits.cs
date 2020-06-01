using EmuTarkov.Common.Utils.App;
using EmuTarkov.Common.Utils.HTTP;
using UnityEngine;

namespace EmuTarkov.SinglePlayer.Utils.Bots
{
    public class BotLimits
    {
        public static BotLimitsData Data { get; private set; }

        public static void RequestData(string session, string backendUrl)
        {
            string json = new Request(session, backendUrl).GetJson("/client/game/bots/limits");

            Data = Json.Deserialize<BotLimitsData>(json);

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
