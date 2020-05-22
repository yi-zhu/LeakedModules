using System.Collections.Generic;
using System.Reflection;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using BotPresets = GClass292;
using BotData = GInterface13;

namespace EmuTarkov.SinglePlayer.Patches.Bots
{
    public class RemoveUsedBotProfilePatch : AbstractPatch
    {
        public static FieldInfo __field;

        public RemoveUsedBotProfilePatch()
        {
            methodName = "GetNewProfile";
            flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        }

        public override MethodInfo TargetMethod()
        {
            var __type = typeof(BotPresets);

            __field = __type.GetField("list_0", BindingFlags.NonPublic | BindingFlags.Instance);
            return __type.GetMethod(methodName, flags);
        }

        public static bool Prefix(ref Profile __result, object __instance, BotData data)
        {
            List<Profile> profiles = (List<Profile>)__field.GetValue(__instance);

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
