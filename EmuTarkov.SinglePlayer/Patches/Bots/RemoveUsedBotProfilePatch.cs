using System.Collections.Generic;
using System.Reflection;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using HarmonyLib;
using System;
using System.Linq;
using BotData = GInterface13;

namespace EmuTarkov.SinglePlayer.Patches.Bots
{
    public class RemoveUsedBotProfilePatch : AbstractPatch
    {
        private static readonly Type targetInterface;
        private static readonly Type targetType;
        private static readonly AccessTools.FieldRef<object, List<Profile>> profilesField;

        static RemoveUsedBotProfilePatch()
        {
            targetInterface = PatcherConstants.TargetAssembly
                .GetTypes()
                .Single(IsTargetInterface);
            targetType = PatcherConstants.TargetAssembly
                .GetTypes()
                .Single(IsTargetType);

            profilesField = AccessTools.FieldRefAccess<List<Profile>>(targetType, "list_0");
        }

        private static bool IsTargetInterface(Type type)
        {
            if (!type.IsInterface)
                return false;
            if (type.GetProperty("StartProfilesLoaded") == null)
                return false;
            if (type.GetMethod("CreateProfile") == null)
                return false;
            return true;
        }

        private static bool IsTargetType(Type type)
        {
            if (!targetInterface.IsAssignableFrom(type))
                return false;
            if (!targetInterface.IsAssignableFrom(type.BaseType))
                return false;
            return true;
        }

        public override MethodInfo TargetMethod()
        {
            return targetType
                .GetMethod("GetNewProfile", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
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
