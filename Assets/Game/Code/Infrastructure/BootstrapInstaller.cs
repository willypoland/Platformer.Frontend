using System;
using Api;
using Game.Code.Data;
using Game.Code.Infrastructure.Services;
using Game.Code.Infrastructure.StateMachine;
using Game.Code.Logic;
using Game.Code.Scene;
using Game.Code.Services;
using Game.Code.UI;
using Game.Code.UI.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Game.Code.Infrastructure
{
    public class BootstrapInstaller : LifetimeScope
    {
        [SerializeField] private GameConfig _config;
        [SerializeField] private CoroutineRunner _coroutineRunner;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            builder.RegisterInstance(_config).AsSelf();
            builder.RegisterInstance(_coroutineRunner).As<ICoroutineRunner>();
            builder.RegisterInstance(FindObjectOfType<ConnectionSetupWindow>()).As<IConnectionSetupView>();
            builder.RegisterInstance(FindObjectOfType<SettingsWindow>()).As<ISettingsWindow>();
            builder.RegisterInstance(FindObjectOfType<ConnectionStatus>()).As<IStatusView>();
            builder.RegisterInstance(FindObjectOfType<SceneViewObjectFactory>()).As<IViewObjectFactory>();
            
            builder.Register<ConnectionArguments>(Lifetime.Singleton).AsSelf();
            builder.Register<GameStateController>(Lifetime.Singleton).As<IStateController>();
            builder.Register<GameSettingsService>(Lifetime.Singleton).AsSelf();
            builder.Register<InputService>(Lifetime.Singleton).As<IInputService>();
            builder.Register<GameplayState>(Lifetime.Singleton).AsSelf();
            builder.Register<ConnectionState>(Lifetime.Singleton).AsSelf();
        }

        private void Start()
        {
            var stateContrller = Container.Resolve<IStateController>();
            stateContrller.Enter<ConnectionState>();
        }
    }


}