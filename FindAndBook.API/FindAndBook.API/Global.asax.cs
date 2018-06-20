using Autofac;
using Autofac.Integration.WebApi;
using Autofac.TypedFactories;
using FindAndBook.API.Controllers;
using FindAndBook.Data;
using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Providers;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services;
using FindAndBook.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace FindAndBook.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            // Data Layer Bindings
            builder.RegisterType<DbContext>()
                .As<IDbContext>()
                .InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .InstancePerRequest();
            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerRequest();

            // Factories
            AutofacTypedFactoryExtensions.RegisterTypedFactory<IUsersFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<IBookingsFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<IRestaurantsFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<IBookedTablesFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<ITablesFactory>(builder).ReturningConcreteType();
            AutofacTypedFactoryExtensions.RegisterTypedFactory<ITokensFactory>(builder).ReturningConcreteType();

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

            builder.RegisterType<JwtSecurityTokenHandler>()
                .AsSelf()
                .InstancePerRequest();
            builder.RegisterType<DateTimeProvider>()
                .As<IDateTimeProvider>()
                .InstancePerRequest();
            builder.RegisterType<AuthenticationProvider>()
                .As<IAuthenticationProvider>()
                .InstancePerRequest();

            builder.RegisterType<TokenValidationHandler>()
                .AsWebApiActionFilterFor<UsersController>()
                .InstancePerRequest();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());    

            var config = GlobalConfiguration.Configuration;
            builder.RegisterWebApiFilterProvider(config);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
