using MessageService.In;
using MessageServiceProvider.@out;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using TimerService;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var props = new NameValueCollection
        {
            { "quartz.serializer.type", "binary" }
        };

        var factory = new StdSchedulerFactory(props);
        var scheduler = factory.GetScheduler().Result;
        Task.Run(() => scheduler.Start());
        var job = JobBuilder.Create<Job>()
            .WithIdentity("myJob", "group1")
            .Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity("IJob", "Job")
            .WithCronSchedule("0 0/1 * 1/1 * ? *")
            .StartAt(DateTime.UtcNow)
            .WithPriority(1)
            .Build();
        Task.Run(() => scheduler.ScheduleJob(job, trigger));
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<Job>();
        serviceCollection.AddScoped<IMessageService, MessageService.Adapter.MessageService>();
        serviceCollection.AddScoped<IMessageServiceProvider, MessageServiceProvider.Adapter.MessageServiceProvider>();
        serviceCollection.AddSingleton(hostContext.Configuration);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        scheduler.JobFactory = new JobFactory(serviceProvider);
    })
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureLogging((hostContext, configLogging) =>
    {
        configLogging.AddConsole();
    });

await builder.Build().RunAsync();