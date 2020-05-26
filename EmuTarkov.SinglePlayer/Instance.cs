using UnityEngine;
using EmuTarkov.SinglePlayer.Patches;
using EmuTarkov.SinglePlayer.Patches.Bots;
using EmuTarkov.SinglePlayer.Patches.Location;
using Comfort.Common;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using EmuTarkov.SinglePlayer.Utils.Bots;
using EmuTarkov.SinglePlayer.Utils.Reflection;
using EmuTarkov.SinglePlayer.Patches.Quests;
using EmuTarkov.SinglePlayer.Patches.Matchmaker;
using EmuTarkov.SinglePlayer.Patches.Weapons;
using EmuTarkov.SinglePlayer.Patches.Dogtag;

namespace EmuTarkov.SinglePlayer
{
    public class Instance : MonoBehaviour
    {
        private void Start()
		{
            Debug.LogError("EmuTarkov.SinglePlayer: Loaded");

            // todo: find a way to get php session id
            BotLimits.RequestData(null, Utils.Config.BackendUrl);

            PatcherUtil.PatchPostfix<BotTemplateLimitPatch>();
            PatcherUtil.PatchPrefix<GetNewBotTemplatesPatch>();
            PatcherUtil.PatchPrefix<RemoveUsedBotProfilePatch>();
            PatcherUtil.PatchPrefix<SpawnPmcPatch>();
            PatcherUtil.PatchPrefix<OfflineLootPatch>();

            PatcherUtil.PatchPrefix<OfflineSaveProfilePatch>();

            PatcherUtil.PatchPrefix<BeaconPatch>();
            PatcherUtil.PatchPostfix<MatchmakerOfflineRaidPatch>();
            PatcherUtil.PatchPostfix<WeaponDurabilityPatch>();

            PatcherUtil.PatchPostfix<DogtagPatch>();
        }
    }
}
