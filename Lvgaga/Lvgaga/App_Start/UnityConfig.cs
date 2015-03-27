using System;
using System.Web.Configuration;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Tumblr;
using LvService.DbContexts;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;
using LvService.Factories.ViewModel;
using LvService.Services;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;

namespace Lvgaga
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterInstance(CloudStorageAccount.Parse(
                WebConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString));
            container.RegisterType<IAzureStorage, AzureStoragePool>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof (AzureStorageDb)));

            const string emptyTumblrReader = "Empty/TumblrReader";
            container.RegisterType<ITableEntitiesCommand, ReadTumblrEntityWithCategoryCommand>(emptyTumblrReader,
                new InjectionConstructor());
            const string homeEntitiesReader = "Tumblr/EntitiesReader";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(homeEntitiesReader,
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<ITableEntitiesCommand>(emptyTumblrReader)));

            container.RegisterType<IUriFactory, UriFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITumblrFactory, TumblrFactory>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof (IUriFactory)));

            container.RegisterType<ITumblrService, TumblrService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof (IAzureStorage),
                    new ResolvedParameter<ITableEntitiesCommand>(homeEntitiesReader),
                    typeof (ITumblrFactory)));
        }
    }
}
