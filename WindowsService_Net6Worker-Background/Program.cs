using Serilog;
using Serilog.Events;
using WindowsService_Net6Worker_Background;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddHostedService<Worker>();
//    })
//    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(@"C:\\Temp\\StartupLog.txt")
    //.CreateLogger();
    .CreateBootstrapLogger();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseSerilog((context, services, configuration) => configuration
            //.WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            //.Enrich.FromLogContext()
            )
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
        });
        //.UseSerilog();

try
{
    Log.Information("Starting up the service");
    var host = CreateHostBuilder(args).Build();
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the serivce");
    throw;
}
finally
{
    Log.Information("Service successfully stopped");
    Log.CloseAndFlush();
}
