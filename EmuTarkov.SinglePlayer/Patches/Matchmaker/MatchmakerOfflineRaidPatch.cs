using EFT.UI;
using EFT.UI.Matchmaker;
using EmuTarkov.Common.Utils.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmuTarkov.SinglePlayer.Patches.Matchmaker
{
    class MatchmakerOfflineRaidPatch : AbstractPatch
    {
        public static void Postfix(UpdatableToggle ____offlineModeToggle, UpdatableToggle ____botsEnabledToggle)
        {
            ____offlineModeToggle.isOn = true;
            ____offlineModeToggle.gameObject.SetActive(false);

            ____botsEnabledToggle.isOn = true;
        }

        public override MethodInfo TargetMethod()
        {
            return typeof(MatchmakerOfflineRaid)
                    .GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}