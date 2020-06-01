using EFT;
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

			new Request(session, backendUrl).PutJson("/raid/profile/save", Json.Serialize(request));
		}

		internal class SaveProfileRequest
		{
			public string exit = "left";
			public Profile profile;
			public bool isPlayerScav;
		}
	}
}
