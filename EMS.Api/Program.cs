using EMS.Api.Extensions;
using EMS.Api.Middleware;
using EMS.Common.Application;
using EMS.Common.Infrastructure;
using EMS.Modules.Events.Infrastructure;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
});

builder.Services.AddApplication([EMS.Modules.Events.Application.AssemblyReference.Assembly]);
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("Database")!,
    builder.Configuration.GetConnectionString("Cache")!);
builder.Configuration.AddModuleConfiguration(["events"]);
builder.Services.AddEventsModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

EventsModule.MapEndPoints(app);

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.Run();
