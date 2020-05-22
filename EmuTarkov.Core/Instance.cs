using UnityEngine;
using EmuTarkov.Core.Patches;
using EmuTarkov.Common.Utils.Patching;

namespace EmuTarkov.Core
{
	public class Instance : MonoBehaviour
	{
		private void Start()
		{
            Debug.LogError("EmuTarkov.Core: Loaded");

            PatcherUtil.PatchPrefix<BattleEyePatch>();
            PatcherUtil.PatchPrefix<SslCertificatePatch>();
            PatcherUtil.PatchPrefix<HttpRequestPatch>();
        }
	}
}
