using UnityEngine;
using EmuTarkov.SinglePlayer.Patches;
using EmuTarkov.SinglePlayer.Patches.Bots;
using EmuTarkov.SinglePlayer.Patches.Location;
using EmuTarkov.SinglePlayer.Monitors;
using Comfort.Common;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using EmuTarkov.SinglePlayer.Utils.Bots;
using EmuTarkov.SinglePlayer.Utils.Reflection;
using EmuTarkov.SinglePlayer.Patches.Quests;
using EmuTarkov.SinglePlayer.Patches.Matchmaker;
using EmuTarkov.SinglePlayer.Patches.Weapons;

namespace EmuTarkov.SinglePlayer
{
    public class Instance : MonoBehaviour
    {
        private void Start()
		{
            Debug.LogError("EmuTarkov.SinglePlayer: Loaded");

            new BotLimits();

            PatcherUtil.PatchPostfix<BotTemplateLimitPatch>();
            PatcherUtil.PatchPrefix<GetNewBotTemplatesPatch>();
            PatcherUtil.PatchPrefix<RemoveUsedBotProfilePatch>();
            PatcherUtil.PatchPrefix<SpawnPmcPatch>();
            PatcherUtil.PatchPrefix<OfflineLootPatch>();

            PatcherUtil.PatchPrefix<OfflineSaveProfilePatch>();

            PatcherUtil.PatchPrefix<BeaconPatch>();
            PatcherUtil.PatchPostfix<MatchmakerOfflineRaidPatch>();
            PatcherUtil.PatchPostfix<WeaponDurabilityPatch>();
        }

        private void FixedUpdate()
        {
            MainApplication mainApplication = ClientAppUtils.GetMainApp();

            if (mainApplication != null)
            {
                BotLimits.RequestData();
            }
        }
	}
}
