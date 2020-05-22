using System;
using System.Reflection;
using Comfort.Common;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using EmuTarkov.SinglePlayer.Utils.Player;
using ISession = GInterface22;
using ClientConfig = GClass266;
using MatchInfo = GClass1240;

namespace EmuTarkov.SinglePlayer.Patches
{
	public class SaveLootPatch : AbstractPatch
	{
		public SaveLootPatch()
		{
			methodName = "method_37";
			flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
		}

		public override void Apply()
		{
			PatcherUtil.PatchPrefix<SaveLootPatch>(TargetMethod());
		}

		public override MethodInfo TargetMethod()
		{
			return PatcherConstants.MainApplicationType.GetMethod(methodName, flags);
		}

		public static void Prefix(ISession ____backEnd, ESideType ___esideType_0, Result<ExitStatus, TimeSpan, MatchInfo> result)
		{
			bool isPlayerScav = false;
			string backendUrl = ClientConfig.Config.BackendUrl;
			var session = ____backEnd.Session;
			Profile profile = ____backEnd.Session.Profile;

			if (___esideType_0 == ESideType.Savage)
			{
				profile = ____backEnd.Session.ProfileOfPet;
				isPlayerScav = true;
			}

			SaveLootUtil.SaveProfileProgress(backendUrl, session.GetPhpSessionId(), result.Value0, profile, isPlayerScav);
		}
	}
}
