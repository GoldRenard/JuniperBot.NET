using JuniperBot.Commands;
using JuniperBot.Services;
using Ninject.Activation.Strategies;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace JuniperBot.Modules {

    internal class DependencyModule : NinjectModule {

        public override void Load() {
            Kernel.Components.Add<IActivationStrategy, ActivationStrategy>();

            Bind<CommandManager>().ToSelf().InSingletonScope().OnActivation(m => m.Initialize());
            Bind<ConfigurationManager>().ToSelf().InSingletonScope().OnActivation(m => m.Initialize());
            Bind<InstagramClient>().ToSelf().InSingletonScope().OnActivation(m => m.Initialize());
            Bind<InstagramPoller>().ToSelf().InSingletonScope().OnActivation(m => m.Initialize());
            Bind<DiscordClient>().ToSelf().InSingletonScope().OnActivation(m => m.Initialize());
            Bind<DiscordWebHookPoster>().ToSelf().InSingletonScope().OnActivation(m => m.Initialize());
            Bind<SchedulerService>().ToSelf().InSingletonScope().OnActivation(m => m.Initialize());

            Kernel.Bind(e => {
                e.FromThisAssembly()
                .IncludingNonePublicTypes()
                .SelectAllClasses()
                .InheritedFrom<ICommand>()
                .BindAllInterfaces()
                .Configure(c => c.InSingletonScope());
            });
        }
    }
}