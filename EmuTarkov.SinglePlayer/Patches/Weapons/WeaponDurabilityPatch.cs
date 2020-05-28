using EFT;
using EmuTarkov.Common.Utils.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using AmmoInfo = GClass1564;

namespace EmuTarkov.SinglePlayer.Patches.Weapons
{
    class WeaponDurabilityPatch : AbstractPatch
    {
        public override MethodInfo TargetMethod()
        {
            //private void method_46(GClass1564 ammo)
            return typeof(Player.FirearmController)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(IsTargetMethod);
        }

        private static bool IsTargetMethod(MethodInfo methodInfo)
        {
            if (methodInfo.IsVirtual)
                return false;

            var parameters = methodInfo.GetParameters();
            if (parameters.Length != 1)
                return false;
            if (parameters[0].ParameterType != typeof(AmmoInfo))
                return false;
            if (parameters[0].Name != "ammo")
                return false;

            var methodBody = methodInfo.GetMethodBody();
            if (methodBody.LocalVariables.Any(x => x.LocalType == typeof(Vector3)))
                return true;

            return false;
        }

        public static void Postfix(Player.FirearmController __instance, AmmoInfo ammo)
        {
            var item = __instance.Item;
            float durability = item.Repairable.Durability;
            
            if (durability <= 0f)
                return;

            float deterioration = ammo.Deterioration;

            float operatingResource = item.Template.OperatingResource > 0
                ? item.Template.OperatingResource
                : 1;

            durability -= item.Repairable.MaxDurability / operatingResource * deterioration;

            item.Repairable.Durability = durability > 0
                ? durability
                : 0;
        }
    }
}
