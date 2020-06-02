﻿using EFT;
using EmuTarkov.Common.Utils.App;
using EmuTarkov.Common.Utils.HTTP;

namespace EmuTarkov.SinglePlayer.Utils.Player
{
	public class SaveLootUtil
	{
		public static void SaveProfileProgress(string backendUrl, string session, ExitStatus exitStatus, Profile profileData, bool isPlayerScav)
		{
			SaveProfileRequest request = new SaveProfileRequest
			{
				exit = exitStatus.ToString().ToLower(),
				profile = profileData,
				isPlayerScav = isPlayerScav
			};

			// ToJson() uses an internal converter which prevents loops and do other internal things
			new Request(session, backendUrl).PutJson("/raid/profile/save", request.ToJson());
		}

		internal class SaveProfileRequest
		{
			public string exit = "left";
			public Profile profile;
			public bool isPlayerScav;
		}
	}
}
