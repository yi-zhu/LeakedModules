﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MainSessionInterface = GInterface22;
using IHealthController = GInterface147;
using MainMenuController = GClass1108;
using StDamage = GStruct203;
using IEffect = GInterface113;
using EFT;
using Comfort.Common;
using EmuTarkov.Common.Utils.HTTP;
using ClientConfig = GClass266;
using Newtonsoft.Json;
using UnityEngine;

namespace EmuTarkov.SinglePlayer.Utils.Player
{
    class HealthListener
    {
        private static object _lock = new object();
        private static HealthListener _instance = null;

        private IHealthController _healthController;
        private bool _inRaid;
        private IDisposable _disposable = null;
        private readonly Request _request;
        private readonly SimpleTimer _simpleTimer;

        public PlayerHealth CurrentHealth { get; } = new PlayerHealth();

        public static HealthListener Instance
        {
            get
            {
                if (_instance == null) {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new HealthListener();
                        }
                    }
                }
                return _instance;
            }
        }

        // ctor
        private HealthListener()
        {
            _request = new Request(Utils.Config.BackEndSession.GetPhpSessionId(), Utils.Config.BackendUrl);
            _simpleTimer = Common.Utils.Hook.Loader<SimpleTimer>.Load();
            _simpleTimer.syncHealthAction = () => Task.Run(() => _request.PostJson("/player/health/sync", CurrentHealth.ToJson()));
        }

        /// <summary>
        /// Initialize HealthListener.
        /// This method is executed on loading profile in menu (on load game, on raid finish, on error...),
        /// and on raid start
        /// </summary>
        /// <param name="healthController">player health controller</param>
        /// <param name="inRaid">true - when executed from raid</param>
        public void Init(IHealthController healthController, bool inRaid)
        {
            // cleanup
            if (_disposable != null)
                _disposable.Dispose();

            // init dependencies
            _healthController = healthController;
            _inRaid = inRaid;

            _simpleTimer.isSyncHealthEnabled = !inRaid;

            CurrentHealth.IsAlive = true;

            // init current health
            SetCurrentHealth(_healthController, CurrentHealth.Health, EBodyPart.Head);
            SetCurrentHealth(_healthController, CurrentHealth.Health, EBodyPart.Chest);
            SetCurrentHealth(_healthController, CurrentHealth.Health, EBodyPart.Stomach);
            SetCurrentHealth(_healthController, CurrentHealth.Health, EBodyPart.LeftArm);
            SetCurrentHealth(_healthController, CurrentHealth.Health, EBodyPart.RightArm);
            SetCurrentHealth(_healthController, CurrentHealth.Health, EBodyPart.LeftLeg);
            SetCurrentHealth(_healthController, CurrentHealth.Health, EBodyPart.RightLeg);

            CurrentHealth.Energy = _healthController.Energy.Current;
            CurrentHealth.Hydration = _healthController.Hydration.Current;

            // subscribe to events
            _healthController.DiedEvent += OnDiedEvent;
            _healthController.HealthChangedEvent += OnHealthChangedEvent;
            _healthController.EffectAddedEvent += OnEffectAddedEvent;
            _healthController.EffectRemovedEvent += OnEffectRemovedEvent;
            _healthController.HydrationChangedEvent += OnHydrationChangedEvent;
            _healthController.EnergyChangedEvent += OnEnergyChangedEvent;

            // don't forget to unsubscribe
            _disposable = new Disposable(() =>
            {
                _healthController.DiedEvent -= OnDiedEvent;
                _healthController.HealthChangedEvent -= OnHealthChangedEvent;
                _healthController.EffectAddedEvent -= OnEffectAddedEvent;
                _healthController.EffectRemovedEvent -= OnEffectRemovedEvent;
                _healthController.HydrationChangedEvent -= OnHydrationChangedEvent;
                _healthController.EnergyChangedEvent -= OnEnergyChangedEvent;
            });
        }

        private void OnDiedEvent(EFT.HealthSystem.EDamageType obj)
        {
            CurrentHealth.IsAlive = false;
        }

        private void SetCurrentHealth(IHealthController healthController, IReadOnlyDictionary<EBodyPart, BodyPartHealth> dictionary, EBodyPart bodyPart)
        {
            var bodyPartHealth = healthController.GetBodyPartHealth(bodyPart);
            dictionary[bodyPart].Initialize(bodyPartHealth.Current, bodyPartHealth.Maximum);
            // set effects
            var effects = healthController.GetActiveEffects(bodyPart);
            if (effects.Any(x => x.Type.Name == "BreakPart"))
                dictionary[bodyPart].AddEffect(BodyPartEffect.BreakPart);
        }

        public void OnHealthChangedEvent(EBodyPart bodyPart, float diff, StDamage effect)
        {
            CurrentHealth.Health[bodyPart].ChangeHealth(diff);
        }

        public void OnEffectAddedEvent(IEffect effect)
        {
            if (effect == null)
                return;
            string effectType = effect.GetType().Name;
            if (effectType != "BreakPart")
                return;

            CurrentHealth.Health[effect.BodyPart].AddEffect(BodyPartEffect.BreakPart);
        }


        public void OnEffectRemovedEvent(IEffect effect)
        {
            if (effect == null)
                return;
            string effectType = effect.GetType().Name;
            if (effectType != "BreakPart")
                return;

            CurrentHealth.Health[effect.BodyPart].RemoveEffect(BodyPartEffect.BreakPart);
        }


        public void OnHydrationChangedEvent(float diff)
        {
            float current = _healthController.Hydration.Current;

            CurrentHealth.Hydration -= diff;
        }


        public void OnEnergyChangedEvent(float diff)
        {
            float current = _healthController.Energy.Current;

            CurrentHealth.Energy -= diff;
        }

        class Disposable : IDisposable
        {
            private readonly Action _onDispose;

            public Disposable(Action onDispose)
            {
                _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
            }

            public void Dispose()
            {
                _onDispose();
            }
        }

        class SimpleTimer : MonoBehaviour
        {
            // tick each 5 seconds
            float sleepTime = 5f;
            float timer = 0f;

            public bool isSyncHealthEnabled = false;
            public Func<Task> syncHealthAction;

            async void Update()
            {
                timer += Time.deltaTime;

                if (timer > sleepTime)
                {
                    timer -= sleepTime;
                    await Tick();
                }
            }

            Task Tick()
            {
                return isSyncHealthEnabled
                    ? syncHealthAction()
                    : Task.CompletedTask;
            }
        }
    }
}