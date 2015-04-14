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

            // Identify
            container.RegisterType<AccountController, AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController, ManageController>(new InjectionConstructor());

            // Cloud
            //container.RegisterInstance(CloudStorageAccount.Parse(
            //    WebConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString));
            container.RegisterInstance(CloudStorageAccount.DevelopmentStorageAccount);
            container.RegisterType<IAzureStorage, AzureStoragePool>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(AzureStorageDb)));

            // Common
            const string emptyEntityReader = "empty://entity/reader";
            container.RegisterType<ITableEntityCommand, ReadTableEntityCommand>(emptyEntityReader, new InjectionConstructor());
            const string emptyEntityDeleter = "empty://entity/deleter";
            container.RegisterType<ICommand, DeleteTableEntityCommand>(emptyEntityDeleter, new InjectionConstructor());
            container.RegisterType<IUriFactory, UriFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITableEntityFactory, TableEntityFactory>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(IUriFactory)));
            container.RegisterType<ISasService, SasService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(IAzureStorage)));

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
                    typeof(ITumblrFactory),
                    typeof(ISasService)));

            // Comment
            const string emptyCommentsReader = "empty://comments/reader";
            container.RegisterType<ITableEntitiesCommand, ReadCommentEntitiesCommand>(emptyCommentsReader,
                new InjectionConstructor());
            const string commentEntitiesReader = "comments://entities/reader";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(commentEntitiesReader,
                new InjectionConstructor(new ResolvedParameter<ITableEntitiesCommand>(emptyCommentsReader)));

            // TODO: add delete favorite command

            container.RegisterType<CreateCommentCommand, CreateCommentCommand>(new InjectionConstructor(),
                new InjectionProperty("TableEntityFactory", typeof(ITableEntityFactory)),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));

            const string createCommentCommand = "post comments://entity";
            container.RegisterType<ICommand, CreateTableEntityCommand>(createCommentCommand,
                new InjectionConstructor(typeof(CreateCommentCommand)));

            container.RegisterType<ICommentFactory, CommentFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICommentService, CommentService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(IAzureStorage),
                    new ResolvedParameter<ICommand>(createCommentCommand),
                    typeof(ITumblrService),
                    new ResolvedParameter<ITableEntityCommand>(emptyEntityReader),
                    new ResolvedParameter<ITableEntitiesCommand>(commentEntitiesReader),
                    typeof(ICommentFactory),
                    typeof(IUriFactory),
                    typeof(ISasService)));

            // Favorite
            container.RegisterType<CreateFavoriteCommand, CreateFavoriteCommand>(new InjectionConstructor(),
                new InjectionProperty("TableEntityFactory", typeof(ITableEntityFactory)),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));

            const string createFavoriteCommand = "post favorites://entity";
            container.RegisterType<ICommand, CreateTableEntitiesCommand>(createFavoriteCommand,
                new InjectionConstructor(typeof(CreateFavoriteCommand)));

            container.RegisterType<ReadPointFavoriteEntitiesCommand, ReadPointFavoriteEntitiesCommand>(new InjectionConstructor(),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));
            const string favoriteEntityReader = "get favorite://entities";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(favoriteEntityReader,
                new InjectionConstructor(typeof(ReadPointFavoriteEntitiesCommand)));

            container.RegisterType<ReadRangeFavoriteEntitiesCommand, ReadRangeFavoriteEntitiesCommand>(new InjectionConstructor(),
                new InjectionProperty("UriFactory", typeof(IUriFactory)));
            const string favoriteRangeEntitiesReader = "get favorites://entities?range";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(favoriteRangeEntitiesReader,
                new InjectionConstructor(typeof(ReadRangeFavoriteEntitiesCommand)));

            const string emptyFavoritesReader = "empty://favorites/reader";
            container.RegisterType<ITableEntitiesCommand, ReadFavoriteEntitiesWithMediaTypeCommand>(emptyFavoritesReader,
                new InjectionConstructor());
            const string favoriteTopEntitiesReader = "get favorites://entities?top";
            container.RegisterType<ITableEntitiesCommand, ReadTableEntitiesCommand>(favoriteTopEntitiesReader,
                new InjectionConstructor(new ResolvedParameter<ITableEntitiesCommand>(emptyFavoritesReader)));

            const string favoriteEntityDeleter = "delete favorite://entities";
            container.RegisterType<ITableEntitiesCommand, DeleteTableEntitiesCommand>(favoriteEntityDeleter,
                new InjectionConstructor(),
                new InjectionProperty("TableEntitiesCommand", new ResolvedParameter<ITableEntitiesCommand>(favoriteEntityReader)));

            container.RegisterType<IFavoriteFactory, FavoriteFactory>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(ITumblrFactory)));

            container.RegisterType<IFavoriteService, FavoriteService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    typeof(IAzureStorage),
                    new ResolvedParameter<ITableEntityCommand>(emptyEntityReader),
                    new ResolvedParameter<ICommand>(createFavoriteCommand),
                    new ResolvedParameter<ITableEntitiesCommand>(favoriteTopEntitiesReader),
                    new ResolvedParameter<ITableEntitiesCommand>(favoriteRangeEntitiesReader),
                    new ResolvedParameter<ITableEntitiesCommand>(favoriteEntityDeleter),
                    typeof(IUriFactory)));
        }
    }
}
