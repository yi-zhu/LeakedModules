using EFT;
using EmuTarkov.Common.Utils.Patching;
using EmuTarkov.SinglePlayer.Utils.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Comfort.Common;
using ClientMetrics = GClass1260;

namespace EmuTarkov.SinglePlayer.Patches
{
    class OfflineSaveProfilePatch : AbstractPatch
    {
        static OfflineSaveProfilePatch()
        {
            // compile-time check
            _ = nameof(ClientMetrics.Metrics);
        }

        public override MethodInfo TargetMethod()
        {
            return PatcherConstants.MainApplicationType
                    .GetMethod("method_37", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static void Prefix(ESideType ___esideType_0, Result<ExitStatus, TimeSpan, ClientMetrics> result)
        {
            string backendUrl = Utils.Config.BackendUrl;

            var session = Utils.Config.BackEndSession;

            bool isPlayerScav = false;

            var profile = session.Profile;
            if (___esideType_0 == ESideType.Savage)
            {
                profile = session.ProfileOfPet;
                isPlayerScav = true;
            }

            SaveLootUtil.SaveProfileProgress(backendUrl, session.GetPhpSessionId(), result.Value0, profile, isPlayerScav);
        }
    }
}
