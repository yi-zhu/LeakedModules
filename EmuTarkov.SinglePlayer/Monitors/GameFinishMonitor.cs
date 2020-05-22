using Comfort.Common;
using EFT;
using EmuTarkov.SinglePlayer.Utils.Player;
using EmuTarkov.SinglePlayer.Utils.Reflection;
using System;
using System.Reflection;
using UnityEngine;
using GameFinish = GClass1230;
using ISession = GInterface23;
using ClientConfig = GClass266;

namespace EmuTarkov.SinglePlayer.Monitors
{
    internal static class GameFinishMonitor
    {
        private static Callback<ExitStatus, TimeSpan, GameFinish> _gameCallBack;

        public static void CheckFinishCallBack(AbstractGame game)
        {
            FieldInfo callBackField = LocalGameUtils.GetFinishCallBack(game);

            if (callBackField == null)
            {
                return;
            }

            if (!(callBackField.GetValue(game) is Callback<ExitStatus, TimeSpan, GameFinish> finishCallBack))
            {
                return;
            }

            if (finishCallBack.Method.Name == "OnGameFinish")
            {
                return;
            }

            _gameCallBack = finishCallBack;
            callBackField.SetValue(game, new Callback<ExitStatus, TimeSpan, GameFinish>(OnGameFinish));
        }

        private static void OnGameFinish(Result<ExitStatus, TimeSpan, GameFinish> result)
        {
            MainApplication mainApplication = ClientAppUtils.GetMainApp();
            ISession session = ClientAppUtils.GetBackendSession();
            string backendUrl = ClientConfig.Config.BackendUrl;

            if (session?.Profile == null || mainApplication == null)
            {
                _gameCallBack(result);
                return;
            }

            ESideType? eSideType = PrivateValueAccessor.GetPrivateFieldValue(mainApplication.GetType(), "esideType_0", mainApplication) as ESideType?;
            Profile profile = session.Profile;
            bool isPlayerScav = false;

            if (eSideType != null && eSideType.GetValueOrDefault() == ESideType.Savage)
            {
                profile = session.ProfileOfPet;
                isPlayerScav = true;
            }

            try
            {
                SaveLootUtil.SaveProfileProgress(backendUrl, session.GetPhpSessionId(), result.Value0, profile, isPlayerScav);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            _gameCallBack(result);
        }
    }
}