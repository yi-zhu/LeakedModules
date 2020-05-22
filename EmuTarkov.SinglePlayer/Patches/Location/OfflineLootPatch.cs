using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EmuTarkov.Common.Utils.HTTP;
using EmuTarkov.Common.Utils.Patching;
using LocationInfo = GClass736.GClass738;
using LocationMatch = GClass736.GClass737;
using JsonInfo = GClass393;

namespace EmuTarkov.SinglePlayer.Patches.Location
{
	public class OfflineLootPatch : AbstractPatch
	{
		public static MethodInfo __method;
		public static PropertyInfo __property;

		public OfflineLootPatch()
		{
			methodName = "method_5";
			flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
		}

		public override MethodInfo TargetMethod()
		{
			var localGameBaseType = PatcherConstants.LocalGameType.BaseType;

			__property = localGameBaseType.GetProperty($"{nameof(GClass736.GClass738)}_0", BindingFlags.NonPublic | BindingFlags.Instance);
			__method = localGameBaseType.GetMethod("smethod_3", BindingFlags.NonPublic | BindingFlags.Static);
			return localGameBaseType.GetMethod(methodName, flags);
		}

		/// <summary>
		/// Loads loot from EmuTarkov's server.
		/// Falls back to the client's local location loot if it fails.
		/// </summary>
		public static bool Prefix(ref Task<LocationInfo> __result, object __instance, string backendUrl)
		{
			if (__instance.GetType() != PatcherConstants.LocalGameType)
			{
				// online match
				return true;
			}

			var location = (LocationInfo)__property.GetValue(__instance);
			var json = new Request(null, backendUrl).GetJson("/api/location/" + location.Id);
			var locationLoot = JsonConvert.DeserializeObject<JsonInfo>(json);

			if (locationLoot == null)
			{
				// failed to download loot
				return true;
			}

			__result = (Task<LocationInfo>)__method.Invoke(null, new[] { new LocationMatch() {
				BackendUrl = backendUrl,
				Location = locationLoot
			}});

			return false;
		}
	}
}