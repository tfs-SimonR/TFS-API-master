using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using TFS_API.ActionFilters;
using TFS_API.Factories;
using TFS_API.Factories.IFactories;
using TFS_API.Models;
using TFS_API.Repositorys;
using TFS_API.Repositorys.Interfaces;
using TFS_API.Services;
using TFS_API.Services.Interfaces;
using Unity;
using Unity.WebApi;
using Microsoft.AspNet.Identity.EntityFramework;
using Serilog;
using Serilog.Events;
using SerilogWeb.Classic;
using TFS_API.Service;
using TFS_API.Service.Interface;

namespace TFS_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            //Trace all requests
            SerilogWebClassic.Configure(cfg => cfg.LogAtLevel(LogEventLevel.Information));



            config.Filters.Add(new LoggingFilterAttribute());
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.MessageHandlers.Add(new LogRequestAndResponseHandler());
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

            // We are using UNITY IOC / DI
            var container = new UnityContainer();
            container.RegisterType<IHandyScannerService, HandyScannerService>();
            container.RegisterType<IExceptionService, ExceptionService>();
            container.RegisterType<IAuditService, AuditService>();
            container.RegisterType<IStoredProcedureService, StoredProcedureService>();
            container.RegisterType<ICountingService, CountingService>();
            container.RegisterType<ICreateFileService, CreateFlatFileService>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IStockService, StockService>();
            container.RegisterType<IClickAndCollectService, ClickAndCollectService>();
            container.RegisterType<IGoogleAPIService, GoogleAPIService>();
            container.RegisterType<IClickAndCollectReserveRepository, ClickAndCollectReserveRepository>();
            container.RegisterType<IClickAndCollectStatusRepository, ClickAndCollectStatusRepository>();
            //container.RegisterType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            container.RegisterType(typeof(IDbFactory<>), typeof(DbFactory<>));
            container.RegisterType<IQueueBusterRepository, QueueBusterRepository>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IClickAndCollectPricingRepository, ClickAndCollectPricingRepository>();
            container.RegisterType<ICashRepository, CashRepository>();
            container.RegisterType<IScannerService, ScannerService>();
            container.RegisterType<IARTSService, ArtsService>();


            //UNITY Dependency Resolver
            config.DependencyResolver = new UnityDependencyResolver(container);


            // Web API Routing (v1 is production)
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
        }
    }
}
