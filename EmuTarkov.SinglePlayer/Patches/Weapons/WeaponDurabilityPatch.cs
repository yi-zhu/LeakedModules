using EFT;
using EmuTarkov.Common.Utils.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EmuTarkov.SinglePlayer.Patches.Weapons
{
    class WeaponDurabilityPatch : AbstractPatch
    {
        public override MethodInfo TargetMethod()
        {
            //private void method_46(GClass1543 ammo)
            return typeof(Player.FirearmController).GetMethod("method_46", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void Postfix(Player.FirearmController __instance, GClass1543 ammo)
        {
            float durability = __instance.Item.Repairable.Durability;
            
            if (durability <= 0f)
                return;

            float deterioration = ammo.Deterioration;

            float operatingResource = __instance.Item.Template.OperatingResource > 0
                ? __instance.Item.Template.OperatingResource
                : 1;

            durability -= __instance.Item.Repairable.MaxDurability / operatingResource * deterioration;

            __instance.Item.Repairable.Durability = durability > 0
                ? durability
                : 0;
        }
    }
}
