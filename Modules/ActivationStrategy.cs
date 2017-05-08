using System;
using JuniperBot.Services;
using Ninject.Activation;
using Ninject.Activation.Strategies;
using Ninject.Components;

namespace JuniperBot.Modules {

    internal class ActivationStrategy : NinjectComponent, IActivationStrategy {
        private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(ActivationStrategy));

        public void Activate(IContext context, InstanceReference reference) {
            Type instanceType = reference.Instance.GetType();
            LOGGER.Info("Component activation: " + instanceType.Name);
            reference.IfInstanceIs<IService>(x => x.Initialize());
        }

        public void Deactivate(IContext context, InstanceReference reference) {
            Type instanceType = reference.Instance.GetType();
            LOGGER.Info("Component deactivation: " + instanceType.Name);
        }
    }
}