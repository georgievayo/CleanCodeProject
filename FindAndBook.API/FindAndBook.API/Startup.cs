using System.IdentityModel.Tokens.Jwt;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Autofac.TypedFactories;
using FindAndBook.Data;
using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Providers;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services;
using FindAndBook.Services.Contracts;
using Microsoft.Owin;
using Owin;

//[assembly: OwinStartup(typeof(FindAndBook.API.Startup))]

namespace FindAndBook.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // Data Layer Bindings
            builder.RegisterType<DbContext>()
                .As<IDbContext>()
                .InstancePerRequest();
            builder.RegisterType(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .InstancePerRequest();
            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerRequest();

            // Service layer Bindings
            builder.RegisterType<RestaurantsService>()
                .As<IRestaurantsService>()
                .InstancePerRequest();
            builder.RegisterType<UsersService>()
                .As<IUsersService>()
                .InstancePerRequest();
            builder.RegisterType<BookingsService>()
                   .As<IBookingsService>()
                   .InstancePerRequest();
            builder.RegisterType<TablesService>()
                .As<ITablesService>()
                .InstancePerRequest();
            builder.RegisterType<BookedTablesService>()
                .As<IBookedTablesService>()
                .InstancePerRequest();
            builder.RegisterType<AuthService>()
                .As<IAuthService>()
                .InstancePerRequest();

            // Factories
            AutofacTypedFactoryExtensions.RegisterTypedFactory<IUsersFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<IBookingsFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<IRestaurantsFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<IBookedTablesFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<ITablesFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<ITokensFactory>(builder).ReturningConcreteType();

            builder.RegisterType<JwtSecurityTokenHandler>()
                .AsSelf()
                .InstancePerRequest();
            builder.RegisterType<AuthenticationProvider>()
                .As<IAuthenticationProvider>()
                .InstancePerRequest();

            var config = GlobalConfiguration.Configuration;
            builder.RegisterWebApiFilterProvider(config);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
