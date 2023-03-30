using Serilog;
using Serilog.Events;
using WindowsService_Net6Worker_Background;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddHostedService<Worker>();
//    })
//    .Build();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
        })
        .UseSerilog();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(@"C:\\Temp\\SeriLogFile-.txt", rollingInterval:RollingInterval.Day)
    //.CreateLogger();
    .CreateBootstrapLogger();

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
