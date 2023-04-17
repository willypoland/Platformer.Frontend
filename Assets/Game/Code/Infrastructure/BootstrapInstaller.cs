using System;
using Game.Code.Data;
using Game.Code.Infrastructure.Services;
using Game.Code.Infrastructure.StateMachine;
using Game.Code.Logic;
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

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            RegisterData(builder);
            RegisterServices(builder);
            RegisterGameStates(builder);
            RegisterUi(builder);
        }

        private void RegisterData(IContainerBuilder builder)
        {
            builder.RegisterInstance(_config).AsSelf();
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<ConnectionArgumentValidator>(Lifetime.Singleton).AsSelf();
            builder.Register<GameStateController>(Lifetime.Singleton).As<IStateController>();
            builder.Register<GameSettingsService>(Lifetime.Singleton).AsSelf();
        }

        private static void RegisterUi(IContainerBuilder builder)
        {
            builder.RegisterInstance(FindObjectOfType<ConnectionSetupWindow>()).As<IConnectionSetupWindow>();
            builder.RegisterInstance(FindObjectOfType<SettingsWindow>()).As<ISettingsWindow>();
            builder.RegisterInstance(FindObjectOfType<ConnectionStatus>()).As<IStatusView>();
        }

        private static void RegisterGameStates(IContainerBuilder builder)
        {
            builder.Register<GameInitializationState>(Lifetime.Singleton).AsSelf();
            builder.Register<ParseCommandLineArgumentState>(Lifetime.Singleton).AsSelf();
        }

        private void Start()
        {
            var stateContrller = Container.Resolve<IStateController>();
            stateContrller.Enter<GameInitializationState>();
        }
    }


}