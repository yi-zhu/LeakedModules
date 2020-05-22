using UnityEngine;
using EmuTarkov.SinglePlayer.Patches.Bots;
using EmuTarkov.SinglePlayer.Patches.Location;
using EmuTarkov.SinglePlayer.Monitors;
using Comfort.Common;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using EmuTarkov.SinglePlayer.Utils.Bots;
using EmuTarkov.SinglePlayer.Utils.Reflection;

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
        }

        private void FixedUpdate()
        {
            MainApplication mainApplication = ClientAppUtils.GetMainApp();
            AbstractGame game = Singleton<AbstractGame>.Instance;

            if (mainApplication != null)
            {
                BotLimits.RequestData();
            }

            if (game != null)
            {
                GameFinishMonitor.CheckFinishCallBack(game);
            }
        }
	}
}
