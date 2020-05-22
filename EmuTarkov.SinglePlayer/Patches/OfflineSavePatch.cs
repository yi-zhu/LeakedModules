using Comfort.Common;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using EmuTarkov.SinglePlayer.Utils.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmuTarkov.SinglePlayer.Patches
{
    class OfflineSaveProfilePatch : AbstractPatch
    {
        public override MethodInfo TargetMethod()
        {
            return PatcherConstants.MainApplicationType
                    .GetMethod("method_37", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static void Prefix(GInterface22 ____backEnd, ESideType ___esideType_0, Result<ExitStatus, TimeSpan, GClass1240> result)
        {
            string backendUrl = GClass266.Config.BackendUrl;

            var session = ____backEnd.Session;

            bool isPlayerScav = false;

            var profile = ____backEnd.Session.Profile;
            if (___esideType_0 == ESideType.Savage)
            {
                profile = ____backEnd.Session.ProfileOfPet;
                isPlayerScav = true;
            }

            SaveLootUtil.SaveProfileProgress(backendUrl, session.GetPhpSessionId(), result.Value0, profile, isPlayerScav);
        }
    }
}
