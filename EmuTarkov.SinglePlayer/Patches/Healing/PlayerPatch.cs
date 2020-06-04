using Comfort.Common;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmuTarkov.SinglePlayer.Patches.Healing
{
    class PlayerPatch : GenericPatch<PlayerPatch>
    {
        private static string _playerAccountId;

        public PlayerPatch() : base(postfix: nameof(PatchPostfix)) { }

        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        static async void PatchPostfix(Player __instance, Task __result)
        {
            if (_playerAccountId == null)
            {
                var backendSession = Utils.Config.BackEndSession;
                Profile profile = backendSession.Profile;
                _playerAccountId = profile.AccountId;
            }

            if (__instance.Profile.AccountId != _playerAccountId)
                return;

            await __result;

            var listener = Utils.Player.HealthListener.Instance;
            listener.Init(__instance.HealthController, true);
        }
    }
}
