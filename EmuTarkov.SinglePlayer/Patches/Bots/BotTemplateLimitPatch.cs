using System.Collections.Generic;
using System.Reflection;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using EmuTarkov.SinglePlayer.Utils.Bots;
using WaveInfo = GClass872;
using BotsPresets = GClass292;

namespace EmuTarkov.SinglePlayer.Patches.Bots
{
    public class BotTemplateLimitPatch : AbstractPatch
    {
        public BotTemplateLimitPatch()
        {
            methodName = "method_1";
            flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        }

        public override MethodInfo TargetMethod()
        {
            return typeof(BotsPresets).GetMethod(methodName, flags);
        }

        public static void Postfix(List<WaveInfo> __result, List<WaveInfo> wavesProfiles, List<WaveInfo> delayed)
        {
            /*
                In short this method sums Limits by grouping wavesPropfiles collection by Role and Difficulty
                then in each group sets Limit to 30, the remainder is stored in "delayed" collection.
                So we change Limit of each group.
                Clear delayed waves, we don't need them if we have enough loaded profiles and in method_2 it creates a lot of garbage.
            */

            delayed?.Clear();
            
            foreach (WaveInfo wave in __result)
            {
                switch (wave.Role)
                {
                    case WildSpawnType.assault:
                        wave.Limit = BotLimits.Data.assault;
                        break;

                    case WildSpawnType.cursedAssault:
                        wave.Limit = BotLimits.Data.cursedAssault;
                        break;

                    case WildSpawnType.marksman:
                        wave.Limit = BotLimits.Data.marksman;
                        break;

                    case WildSpawnType.pmcBot:
                        wave.Limit = BotLimits.Data.pmcBot;
                        break;

                    case WildSpawnType.bossBully:
                        wave.Limit = BotLimits.Data.bossBully;
                        break;

                    case WildSpawnType.bossGluhar:
                        wave.Limit = BotLimits.Data.bossGluhar;
                        break;

                    case WildSpawnType.bossKilla:
                        wave.Limit = BotLimits.Data.bossKilla;
                        break;

                    case WildSpawnType.bossKojaniy:
                        wave.Limit = BotLimits.Data.bossKojaniy;
                        break;

                    case WildSpawnType.bossStormtrooper:
                        wave.Limit = BotLimits.Data.bossStormtrooper;
                        break;

                    case WildSpawnType.bossTest:
                        wave.Limit = BotLimits.Data.bossTest;
                        break;

                    case WildSpawnType.followerBully:
                        wave.Limit = BotLimits.Data.followerBully;
                        break;

                    case WildSpawnType.followerGluharAssault:
                        wave.Limit = BotLimits.Data.followerGluharAssault;
                        break;

                    case WildSpawnType.followerGluharScout:
                        wave.Limit = BotLimits.Data.followerGluharScout;
                        break;

                    case WildSpawnType.followerGluharSecurity:
                        wave.Limit = BotLimits.Data.followerGluharSecurity;
                        break;

                    case WildSpawnType.followerGluharSnipe:
                        wave.Limit = BotLimits.Data.followerGluharSnipe;
                        break;

                    case WildSpawnType.followerKojaniy:
                        wave.Limit = BotLimits.Data.followerKojaniy;
                        break;

                    case WildSpawnType.followerStormtrooper:
                        wave.Limit = BotLimits.Data.followerStormtrooper;
                        break;

                    case WildSpawnType.followerTest:
                        wave.Limit = BotLimits.Data.followerTest;
                        break;
                }
            }
        }
    }
}
