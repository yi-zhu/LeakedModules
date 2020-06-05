using System;
using System.Collections.Generic;
using EmuTarkov.Common.Utils.HTTP;
using UnityEngine;
using EFT;

namespace EmuTarkov.SinglePlayer.Utils.Bots
{
    public class BotSettings
    {
		private static string Session;
		private static string BackendUrl;

		public static Dictionary<WildSpawnType, int> Limits { get; private set; }
		public static List<Difficulty> Difficulties { get; private set; }
		public static string CoreDifficulty { get; private set; }

		public BotSettings(string session, string backendUrl)
		{
			Limits = new Dictionary<WildSpawnType, int>();
			Difficulties = new List<Difficulty>();
			Session = session;
			BackendUrl = backendUrl;

			// set core values
			CoreDifficulty = null;
			RequestCoreDifficulty();

			// set bot values
			var types = Enum.GetValues(typeof(WildSpawnType));
			var difficulties = Enum.GetValues(typeof(BotDifficulty));

			foreach (WildSpawnType type in types)
			{
				// set default values
				Limits[type] = 30;
				RequestLimit(type);

				foreach (BotDifficulty botDifficulty in difficulties)
				{
					Difficulty difficulty = new Difficulty(type, botDifficulty, null);
					Difficulties.Add(difficulty);
					RequestDifficulty(type, botDifficulty, difficulty);
				}
			}
		}

		private static void RequestLimit(WildSpawnType role)
		{
			string json = new Request(Session, BackendUrl).GetJson("/client/game/bot/limit/" + role.ToString());

			if (string.IsNullOrEmpty(json))
			{
				Debug.LogError("EmuTarkov.SinglePlayer: Received bot " + role.ToString() + " limit data is NULL, using fallback");
				return;
			}

			Debug.LogError("EmuTarkov.SinglePlayer: Sucessfully received bot " + role.ToString() + " limit data");
			Limits.Add(role, Convert.ToInt32(json));
		}

		private static void RequestDifficulty(WildSpawnType role, BotDifficulty botDifficulty, Difficulty difficulty)
		{
			string json = new Request(Session, BackendUrl).GetJson("/client/game/bot/difficulty/" + role.ToString() + "/" + botDifficulty.ToString());

			if (string.IsNullOrEmpty(json))
			{
				Debug.LogError("EmuTarkov.SinglePlayer: Received bot " + role.ToString() + " " + botDifficulty.ToString() + " difficulty data is NULL, using fallback");
				return;
			}

			Debug.LogError("EmuTarkov.SinglePlayer: Sucessfully received bot " + role.ToString() + " " + botDifficulty.ToString() + " difficulty data");
			difficulty.Json = json;
		}

		private static void RequestCoreDifficulty()
		{
			string json = new Request(Session, BackendUrl).GetJson("/client/game/bot/difficulty/core/core");

			if (string.IsNullOrEmpty(json))
			{
				Debug.LogError("EmuTarkov.SinglePlayer: Received core bot difficulty data is NULL, using fallback");
				return;
			}

			Debug.LogError("EmuTarkov.SinglePlayer: Sucessfully core bot difficulty data");
			CoreDifficulty = json;
		}
	}
}
