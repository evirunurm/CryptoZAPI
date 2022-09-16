using Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.AddHostedService<AutoUpdate>();
    })
    .Build();

await host.RunAsync();
