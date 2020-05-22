using System.Reflection;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using BotSpawner = GClass294;

namespace EmuTarkov.SinglePlayer.Patches.Bots
{
    public class SpawnPmcPatch : AbstractPatch
    {
        public SpawnPmcPatch()
        {
            methodName = "method_0";
            flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        }

        public override MethodInfo TargetMethod()
        {
            return typeof(BotSpawner).GetMethod(methodName, flags);
        }

        public static bool Prefix(ref bool __result, Profile x, WildSpawnType ___Type, BotDifficulty ___BotDifficulty)
        {
            __result = x.Info.Settings.Role == ___Type && x.Info.Settings.BotDifficulty == ___BotDifficulty;

            return false;
        }
    }
}
