using System;
using Lvgaga.Controllers;
using LvModel.Azure.StorageTable;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Common;
using LvService.Commands.Lvgaga.Comment;
using LvService.Commands.Lvgaga.Favlrite;
using LvService.Commands.Lvgaga.Tumblr;
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

            #region Identity

            container.RegisterType<AccountController, AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController, ManageController>(new InjectionConstructor());

            #endregion

            #region Common

            container.RegisterType<IUriFactory, UriFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICacheKeyFactory, CacheKeyFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICache, LvMemoryCache>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITableEntityFactory, TableEntityFactory>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(IUriFactory)));

            #endregion

            #region Cloud

            //container.RegisterInstance(CloudStorageAccount.Parse(
            //    WebConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString));
            container.RegisterInstance(CloudStorageAccount.DevelopmentStorageAccount);
            container.RegisterType<IAzureStorage, AzureStoragePool>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(AzureStorageDb),
                    typeof(ICache),
                    typeof(ICacheKeyFactory)));

            #endregion

            #region Sas

            container.RegisterType<SasService, SasService>(new InjectionConstructor(typeof(IAzureStorage)));
            container.RegisterType<ISasService, CachedSasService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(SasService),
                    typeof(ICache),
                    typeof(ICacheKeyFactory)));

            #endregion

            #region Tumblr

            container.RegisterType<ITumblrFactory, TumblrFactory>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(IUriFactory)));

            const string getTumblrsCommand = "get tumblrs://entities";
            container.RegisterType<ICommand, CompositeCommand>(
                getTumblrsCommand,
                new InjectionConstructor(
                    new ResolvedArrayParameter<ICommand>(
                        typeof(CreateTumblrEntitiesFilterCommand),
                        typeof(ReadTableEntitiesCommand<TumblrEntity>))));

            container.RegisterType<TumblrService, TumblrService>(
                new InjectionConstructor(
                    typeof(IAzureStorage),
                    typeof(ReadTableEntityCommand<TumblrEntity>),
                    new ResolvedParameter<ICommand>(getTumblrsCommand),
                    typeof(ITumblrFactory),
                    typeof(ISasService)));
            container.RegisterType<ITumblrService, CachedTumblrService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(TumblrService),
                    typeof(ICache),
                    typeof(ICacheKeyFactory)));

            #endregion

            #region Comment

            container.RegisterType<CreateCommentCommand, CreateCommentCommand>(
                new InjectionConstructor(),
                new InjectionProperty("TableEntityFactory", typeof(ITableEntityFactory)),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));
            const string createCommentCommand = "post comment://entity";
            container.RegisterType<ICommand, CompositeCommand>(
                createCommentCommand,
                new InjectionConstructor(
                    new ResolvedArrayParameter<ICommand>(
                        typeof(CreateCommentCommand),
                        typeof(CreateTableEntityCommand))));

            container.RegisterType<ICommentFactory, CommentFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<SasCommentService, SasCommentService>(
                new InjectionConstructor(
                    typeof(IAzureStorage),
                    new ResolvedParameter<ICommand>(createCommentCommand),
                    typeof(ITumblrService),
                    typeof(ICommentFactory),
                    typeof(IUriFactory),
                    typeof(ISasService)));
            container.RegisterType<ICommentService, CachedCommentService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(SasCommentService), typeof(ICache), typeof(ICacheKeyFactory)));

            #endregion

            #region Favorite

            container.RegisterType<CreateFavoriteCommand, CreateFavoriteCommand>(
                new InjectionConstructor(),
                new InjectionProperty("TableEntityFactory", typeof(ITableEntityFactory)),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));

            const string createFavoriteCommand = "post favorite://entities";
            container.RegisterType<ICommand, CompositeCommand>(
                createFavoriteCommand,
                new InjectionConstructor(
                    new ResolvedArrayParameter<ICommand>(
                        typeof(CreateFavoriteCommand),
                        typeof(CreateTableEntitiesCommand))));

            container.RegisterType<DeleteFavoriteCommand, DeleteFavoriteCommand>(
                new InjectionConstructor(),
                new InjectionProperty("TableEntityFactory", typeof(ITableEntityFactory)),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));

            const string deleteFavoriteCommand = "delete favorite://entities";
            container.RegisterType<ICommand, CompositeCommand>(
                deleteFavoriteCommand,
                new InjectionConstructor(
                    new ResolvedArrayParameter<ICommand>(
                        typeof(DeleteFavoriteCommand),
                        typeof(DeleteTableEntitiesCommand))));

            container.RegisterType<IFavoriteService, FavoriteService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(IAzureStorage),
                    typeof(ReadTableEntityCommand<TumblrEntity>),
                    new ResolvedParameter<ICommand>(createFavoriteCommand),
                    new ResolvedParameter<ICommand>(deleteFavoriteCommand),
                    typeof(IUriFactory)));

            #endregion
        }
    }
}