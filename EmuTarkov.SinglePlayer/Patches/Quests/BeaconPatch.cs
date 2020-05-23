using EFT;
using EFT.InventoryLogic;
using EmuTarkov.Common.Utils.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmuTarkov.SinglePlayer.Patches.Quests
{
    public class BeaconPatch : AbstractPatch
    {
        public override MethodInfo TargetMethod()
        {
            return typeof(Player)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Single(IsTargetMethod);
        }

        private bool IsTargetMethod(MethodInfo method)
        {
            if (!method.IsVirtual)
                return false;
            var parameters = method.GetParameters();
            if (parameters.Length != 2)
                return false;
            if (parameters[0].ParameterType != typeof(Item))
                return false;
            if (parameters[0].Name != "item")
                return false;
            if (parameters[1].ParameterType != typeof(string))
                return false;
            if (parameters[1].Name != "zone")
                return false;
            return true;
        }

        public static bool Prefix(Player __instance, Item item, string zone)
        {
            __instance.Profile.ItemDroppedAtPlace(item.TemplateId, zone);

            return false;
        }
    }
}