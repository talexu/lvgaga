using System;
using System.Web.Configuration;
using Lvgaga.Controllers;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Common;
using LvService.Commands.Tumblr;
using LvService.DbContexts;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;
using LvService.Factories.ViewModel;
using LvService.Services;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;

namespace Lvgaga.App_Start
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

            // Identify
            container.RegisterType<AccountController, AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController, ManageController>(new InjectionConstructor());

            // Cloud
            container.RegisterInstance(CloudStorageAccount.Parse(
                WebConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString));
            //container.RegisterInstance(CloudStorageAccount.DevelopmentStorageAccount);
            container.RegisterType<IAzureStorage, AzureStoragePool>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(AzureStorageDb)));

            // Common
            const string emptyEntityReader = "empty://entity/reader";
            container.RegisterType<ITableEntityCommand, ReadTableEntityCommand>(emptyEntityReader,
                new InjectionConstructor());
            container.RegisterType<IUriFactory, UriFactory>(new ContainerControlledLifetimeManager());

            // Tumblr
            const string emptyTumblrsReader = "empty://tumblrs/reader";
            container.RegisterType<ITableEntitiesCommand, ReadTumblrEntitiesWithCategoryCommand>(emptyTumblrsReader,
                new InjectionConstructor());
            const string homeEntitiesReader = "tumblr://entities/reader";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(homeEntitiesReader,
                new InjectionConstructor(new ResolvedParameter<ITableEntitiesCommand>(emptyTumblrsReader)));

            container.RegisterType<ITumblrFactory, TumblrFactory>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(IUriFactory)));

            container.RegisterType<ITumblrService, TumblrService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(IAzureStorage),
                    new ResolvedParameter<ITableEntityCommand>(emptyEntityReader),
                    new ResolvedParameter<ITableEntitiesCommand>(homeEntitiesReader),
                    typeof(ITumblrFactory)));

            // Comment
            const string emptyCommentsReader = "empty://comments/reader";
            container.RegisterType<ITableEntitiesCommand, ReadCommentEntitiesCommand>(emptyCommentsReader,
                new InjectionConstructor());
            const string commentEntitiesReader = "comments://entities/reader";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(commentEntitiesReader,
                new InjectionConstructor(new ResolvedParameter<ITableEntitiesCommand>(emptyCommentsReader)));
            container.RegisterType<CreateCommentCommand, CreateCommentCommand>(new InjectionConstructor(),
                new InjectionProperty("TableEntityFactory", typeof (TableEntityFactory)));
            const string createCommentCommand = "post:comments://entity";
            container.RegisterType<ICommand, CreateTableEntityCommand>(createCommentCommand,
                new InjectionConstructor(typeof(CreateCommentCommand)));

            container.RegisterType<ICommentFactory, CommentFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICommentService, CommentService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(IAzureStorage),
                    new ResolvedParameter<ICommand>(createCommentCommand),
                    typeof(ITumblrService),
                    new ResolvedParameter<ITableEntitiesCommand>(commentEntitiesReader),
                    typeof(ICommentFactory)));
        }
    }
}
