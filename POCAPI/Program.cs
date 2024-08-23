using Hangfire;
using Hangfire.MemoryStorage;
using POC.Application.Interfaces;
using POC.Application.Services;
using POC.Application.Settings;
using POC.Domain.Repositories;
using POC.Domain.Settings;
using POC.Infra.Persistence;
using POC.Infra.Repositories;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<MongoDbContext>();

// Repositories
builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();

// Services
builder.Services.AddScoped<SensorAnalysisJobService>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();

builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseDefaultTypeSerializer()
          .UseMemoryStorage());

builder.Services.AddHangfireServer();


var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MeasurementConsumer>(); 

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        cfg.ReceiveEndpoint("measurement-queue", e =>
        {
            e.ConfigureConsumer<MeasurementConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();


var app = builder.Build();

// Agendar o JOB
using (var scope = app.Services.CreateScope())
{
    var sensorAnalysisJobService = scope.ServiceProvider.GetRequiredService<SensorAnalysisJobService>();
    sensorAnalysisJobService.ScheduleSensorAnalysisJob();
}

app.UseHangfireDashboard("/hangfire");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "POC API V1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
