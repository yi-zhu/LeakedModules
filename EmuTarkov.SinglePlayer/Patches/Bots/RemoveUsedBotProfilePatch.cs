using System.Collections.Generic;
using System.Reflection;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using BotPresets = GClass292;
using BotData = GInterface13;
using HarmonyLib;
using System;

namespace EmuTarkov.SinglePlayer.Patches.Bots
{
    public class RemoveUsedBotProfilePatch : AbstractPatch
    {
        private static readonly Type targetType;
        private static readonly AccessTools.FieldRef<object, List<Profile>> profilesField;

        static RemoveUsedBotProfilePatch()
        {
            targetType = typeof(BotPresets);
            profilesField = AccessTools.FieldRefAccess<List<Profile>>(targetType, "list_0");
        }

        public override MethodInfo TargetMethod()
        {
            return targetType.GetMethod("GetNewProfile", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static bool Prefix(ref Profile __result, object __instance, BotData data)
        {
            List<Profile> profiles = profilesField(__instance);

            if (profiles.Count > 0)
            {
                // second parameter makes client remove used profiles
                __result = data.ChooseProfile(profiles, true);
            }
            else
            {
                __result = null;
            }

            return false;
        }
    }
}
