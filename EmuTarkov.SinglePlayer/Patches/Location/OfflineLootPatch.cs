using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EmuTarkov.Common.Utils.HTTP;
using EmuTarkov.Common.Utils.Patching;
using LocationInfo = GClass736.GClass738;
using LocationMatch = GClass736.GClass737;
using JsonInfo = GClass393;
using UnityEngine;
using System;
using EFT.InventoryLogic;
using System.Linq;

namespace EmuTarkov.SinglePlayer.Patches.Location
{
	public class OfflineLootPatch : AbstractPatch
	{
		public static PropertyInfo _property;

		public OfflineLootPatch()
		{
			methodName = "method_5";
			flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
		}

		public override MethodInfo TargetMethod()
		{
			var localGameBaseType = PatcherConstants.LocalGameType.BaseType;

			_property = localGameBaseType.GetProperty($"{nameof(GClass736.GClass738)}_0", BindingFlags.NonPublic | BindingFlags.Instance);
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
				Debug.LogError("OfflineLootPatch > Online match?!");
				return true;
			}

			var location = (LocationInfo)_property.GetValue(__instance);
			var json = new Request(Utils.Config.BackEndSession.GetPhpSessionId(), backendUrl).GetJson("/api/location/" + location.Id);
			var locationLoot = json.ParseJsonTo<LocationInfo>(Array.Empty<JsonConverter>());

			// uncomment to dump client location data
			/*
			foreach (var num in Enumerable.Range(1, 6))
			{
				var loot = GClass407.Load<TextAsset>($"LocalLoot/{location.Id}{num}").text;
				System.IO.File.WriteAllText($"{___gclass738_0.Id}_{num}.json", loot);
			}
			*/

			if (locationLoot == null)
			{
				// failed to download loot
				Debug.LogError("OfflineLootPatch > Failed to download loot, using fallback");
				return true;
			}

			Debug.LogError("OfflineLootPatch > Successfully received loot from server");
			__result = Task.FromResult(locationLoot);

			return false;
		}
	}
}