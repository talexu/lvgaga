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

            const string emptyEntityReader = "get entity://";
            container.RegisterType<ITableEntityCommand, ReadTableEntityCommand>(emptyEntityReader,
                new InjectionConstructor());
            const string emptyEntityDeleter = "delete entity://";
            container.RegisterType<ICommand, DeleteTableEntityCommand>(emptyEntityDeleter, new InjectionConstructor());
            container.RegisterType<IUriFactory, UriFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICacheKeyFactory, CacheKeyFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITableEntityFactory, TableEntityFactory>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(IUriFactory)));
            container.RegisterType<SasService, SasService>(new InjectionConstructor(typeof(IAzureStorage)));
            container.RegisterType<ISasService, CachedSasService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(SasService), typeof(ICache), typeof(ICacheKeyFactory)));
            container.RegisterType<ICache, LvMemoryCache>(new ContainerControlledLifetimeManager());

            #endregion

            #region Cloud

            container.RegisterInstance(CloudStorageAccount.Parse(
                WebConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString));
            //container.RegisterInstance(CloudStorageAccount.DevelopmentStorageAccount);
            container.RegisterType<IAzureStorage, AzureStoragePool>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(AzureStorageDb), typeof(ICache), typeof(ICacheKeyFactory)));

            #endregion

            #region Tumblr

            const string emptyTumblrsReader = "get tumblr://category";
            container.RegisterType<ITableEntitiesCommand, ReadTumblrEntitiesWithCategoryCommand>(emptyTumblrsReader,
                new InjectionConstructor());
            const string homeEntitiesReader = "get tumblr://entities";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(homeEntitiesReader,
                new InjectionConstructor(new ResolvedParameter<ITableEntitiesCommand>(emptyTumblrsReader)));

            container.RegisterType<ITumblrFactory, TumblrFactory>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(IUriFactory)));

            container.RegisterType<TumblrService, TumblrService>(new InjectionConstructor(
                typeof(IAzureStorage),
                new ResolvedParameter<ITableEntityCommand>(emptyEntityReader),
                new ResolvedParameter<ITableEntitiesCommand>(homeEntitiesReader),
                typeof(ITumblrFactory),
                typeof(ISasService)));
            container.RegisterType<ITumblrService, CachedTumblrService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(TumblrService), typeof(ICache), typeof(ICacheKeyFactory)));

            #endregion

            #region Comment

            const string emptyCommentsReader = "get comment://tumblr";
            container.RegisterType<ITableEntitiesCommand, ReadCommentEntitiesCommand>(emptyCommentsReader,
                new InjectionConstructor());
            const string commentEntitiesReader = "get comment://entities";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(commentEntitiesReader,
                new InjectionConstructor(new ResolvedParameter<ITableEntitiesCommand>(emptyCommentsReader)));

            container.RegisterType<CreateCommentCommand, CreateCommentCommand>(new InjectionConstructor(),
                new InjectionProperty("TableEntityFactory", typeof(ITableEntityFactory)),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));

            const string createCommentCommand = "post comment://entity";
            container.RegisterType<ICommand, CreateTableEntityCommand>(createCommentCommand,
                new InjectionConstructor(typeof(CreateCommentCommand)));

            container.RegisterType<ICommentFactory, CommentFactory>(new ContainerControlledLifetimeManager());

            //container.RegisterType<ICommentService, CommentService>(new ContainerControlledLifetimeManager(),
            //    new InjectionConstructor(
            //        typeof(IAzureStorage),
            //        new ResolvedParameter<ICommand>(createCommentCommand),
            //        typeof(ITumblrService),
            //        new ResolvedParameter<ITableEntityCommand>(emptyEntityReader),
            //        new ResolvedParameter<ITableEntitiesCommand>(commentEntitiesReader),
            //        typeof(ICommentFactory),
            //        typeof(IUriFactory),
            //        typeof(ISasService)));
            container.RegisterType<SasCommentService, SasCommentService>(new InjectionConstructor(
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

            container.RegisterType<CreateFavoriteCommand, CreateFavoriteCommand>(new InjectionConstructor(),
                new InjectionProperty("TableEntityFactory", typeof(ITableEntityFactory)),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));

            const string createFavoriteCommand = "post favorite://entity";
            container.RegisterType<ICommand, CreateTableEntitiesCommand>(createFavoriteCommand,
                new InjectionConstructor(typeof(CreateFavoriteCommand)));

            container.RegisterType<ReadPointFavoriteEntitiesCommand, ReadPointFavoriteEntitiesCommand>(
                new InjectionConstructor(),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));
            const string favoriteEntityReader = "get favorite://entities";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(favoriteEntityReader,
                new InjectionConstructor(typeof(ReadPointFavoriteEntitiesCommand)));

            container.RegisterType<ReadRangeFavoriteEntitiesCommand, ReadRangeFavoriteEntitiesCommand>(
                new InjectionConstructor(),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));
            const string favoriteRangeEntitiesReader = "get favorite://entities/range";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(favoriteRangeEntitiesReader,
                new InjectionConstructor(typeof(ReadRangeFavoriteEntitiesCommand)));

            const string emptyFavoritesReader = "get favorite://mediatype";
            container.RegisterType<ITableEntitiesCommand, ReadFavoriteEntitiesWithMediaTypeCommand>(
                emptyFavoritesReader,
                new InjectionConstructor());
            const string favoriteTopEntitiesReader = "get favorite://entities/top";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(favoriteTopEntitiesReader,
                new InjectionConstructor(new ResolvedParameter<ITableEntitiesCommand>(emptyFavoritesReader)));

            const string favoriteEntityDeleter = "delete favorite://entities";
            container.RegisterType<ITableEntitiesCommand, DeleteTableEntitiesCommand>(favoriteEntityDeleter,
                new InjectionConstructor(),
                new InjectionProperty("TableEntitiesCommand",
                    new ResolvedParameter<ITableEntitiesCommand>(favoriteEntityReader)));

            container.RegisterType<IFavoriteFactory, FavoriteFactory>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(ITumblrFactory)));

            container.RegisterType<FavoriteService, FavoriteService>(new InjectionConstructor(
                typeof(IAzureStorage),
                new ResolvedParameter<ITableEntityCommand>(emptyEntityReader),
                new ResolvedParameter<ICommand>(createFavoriteCommand),
                new ResolvedParameter<ITableEntitiesCommand>(favoriteTopEntitiesReader),
                new ResolvedParameter<ITableEntitiesCommand>(favoriteRangeEntitiesReader),
                new ResolvedParameter<ITableEntitiesCommand>(favoriteEntityDeleter),
                typeof(IUriFactory)));
            container.RegisterType<IFavoriteService, CachedFavoriteService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(FavoriteService), typeof(ICache), typeof(ICacheKeyFactory)));

            #endregion
        }
    }
}