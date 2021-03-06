using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace Octothorp.Home
{
    public class ApiModule : Module
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;

        public ApiModule(IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterLogger(builder);
        }

        private void RegisterLogger(ContainerBuilder containerBuilder)
        {
            LoggerConfiguration builder = new LoggerConfiguration();

            builder
                .ReadFrom
                .Configuration(_configuration);

            builder
                .Enrich.WithUserName()
                .Enrich.WithMachineName()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .Enrich.FromLogContext();

            Logger logger = builder.CreateLogger();
            Log.Logger = logger;

            containerBuilder.Register(f => logger).As<ILogger>().SingleInstance();
        }
    }
}