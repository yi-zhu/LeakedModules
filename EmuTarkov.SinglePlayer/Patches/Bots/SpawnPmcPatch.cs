using System.Reflection;
using HarmonyLib;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using BotSpawner = GClass294;
using System;

namespace EmuTarkov.SinglePlayer.Patches.Bots
{
    public class SpawnPmcPatch : AbstractPatch
    {
        private static readonly Type targetType;
        private static readonly AccessTools.FieldRef<object, WildSpawnType> wildSpawnTypeField;
        private static readonly AccessTools.FieldRef<object, BotDifficulty> botDifficultyField;

        static SpawnPmcPatch()
        {
            targetType = typeof(BotSpawner);
            wildSpawnTypeField = AccessTools.FieldRefAccess<WildSpawnType>(targetType, "Type");
            botDifficultyField = AccessTools.FieldRefAccess<BotDifficulty>(targetType, "BotDifficulty");
        }

        public override MethodInfo TargetMethod()
        {
            return typeof(BotSpawner).GetMethod("method_0", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static bool Prefix(object __instance, ref bool __result, Profile x)
        {
            var botType = wildSpawnTypeField(__instance);
            var botDifficulty = botDifficultyField(__instance);

            __result = x.Info.Settings.Role == botType && x.Info.Settings.BotDifficulty == botDifficulty;

            return false;
        }
    }
}
