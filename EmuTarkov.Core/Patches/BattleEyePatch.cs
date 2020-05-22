using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BattlEye;
using EmuTarkov.Common.Utils.Patching;

namespace EmuTarkov.Core.Patches
{
	public class BattleEyePatch : AbstractPatch
	{
        public static PropertyInfo __property;

        public BattleEyePatch()
		{
			methodName = "RunValidation";
			flags = BindingFlags.Public | BindingFlags.Instance;
		}

		public override MethodInfo TargetMethod()
		{
            var __type = PatcherConstants.TargetAssembly.GetTypes().Single(x => x.GetMethod(methodName, flags) != null);

            __property = __type.GetProperty("Succeed", flags);
            return __type.GetMethod(methodName, flags);
        }

		public static bool Prefix(ref Task __result, object __instance, BEClient.LogDelegate logDelegate)
		{
            __property.SetValue(__instance, true);
			__result = Task.CompletedTask;

			return false;
		}
	}
}
