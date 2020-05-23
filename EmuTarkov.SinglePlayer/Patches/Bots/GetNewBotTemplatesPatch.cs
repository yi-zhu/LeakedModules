using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using Comfort.Common;
using EFT;
using EmuTarkov.Common.Utils.Patching;
using ISession = GInterface23;
using WaveInfo = GClass872;
using BotsPresets = GClass292;
using BotData = GInterface13;
using PoolManager = GClass1088;
using JobPriority = GClass592;

namespace EmuTarkov.SinglePlayer.Patches.Bots
{
    public class GetNewBotTemplatesPatch : AbstractPatch
    {
        public static FieldInfo __field;

        public GetNewBotTemplatesPatch()
        {
            methodName = "method_2";
            flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        }

        public override MethodInfo TargetMethod()
        {
            var __type = typeof(BotsPresets);

            __field = __type.GetField("ginterface23_0", flags);
            return __type.GetMethod(methodName, flags);
        }

        public static bool Prefix(ref Task<Profile> __result, BotsPresets __instance, BotData data)
        {
            /*
                in short when client wants new bot and GetNewProfile() return null (if not more available templates or they don't satisfied by Role and Difficulty condition)
                then client gets new piece of WaveInfo collection (with Limit = 30 by default) and make request to server
                but use only first value in response (this creates a lot of garbage and cause freezes)
                after patch we request only 1 template from server

                along with other patches this one causes to call data.PrepareToLoadBackend(1) gets the result with required role and difficulty:
                new[] { new WaveInfo() { Limit = 1, Role = role, Difficulty = difficulty } }
                then perform request to server and get only first value of resulting single element collection
            */

            ISession session = (ISession)__field.GetValue(__instance);
            Task<Profile> taskAwaiter = null;
            TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            if (session != null)
            {
                // try get profile from cache
                Profile profile = __instance.GetType()
                    .GetMethod("GetNewProfile", BindingFlags.NonPublic | BindingFlags.Instance)
                    .Invoke(__instance, new[] { data }) as Profile;

                if (profile == null)
                {
                    // load from server
                    Debug.LogError("EmuTarkov.SinglePlayer: Loading bot profile from server");

                    List<WaveInfo> source = data.PrepareToLoadBackend(1).ToList();
                    taskAwaiter = session.LoadBots(source).ContinueWith(t => t.Result[0], taskScheduler);
                }
                else
                {
                    // return cached profile
                    Debug.LogError("EmuTarkov.SinglePlayer: Loading bot profile from cache");
                    taskAwaiter = Task.FromResult(profile);
                }
            }

            // load bundles for bot profile
            __result = taskAwaiter.ContinueWith(t =>
            {
                Profile profile = t.Result;
                Task loadTask = Singleton<PoolManager>.Instance
                    .LoadBundlesAndCreatePools(PoolManager.PoolsCategory.Raid, PoolManager.AssemblyType.Local, profile.GetAllPrefabPaths(false)
                    .ToArray(), JobPriority.General, null, default);

                return loadTask.ContinueWith(t2 => profile, taskScheduler);
            }, taskScheduler).Unwrap();

            return false;
        }
    }
}
