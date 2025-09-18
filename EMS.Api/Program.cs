using EMS.Api.Extensions;
using EMS.Api.Middleware;
using EMS.Common.Application;
using EMS.Common.Infrastructure;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Attendance.Infrastructure;
using EMS.Modules.Events.Infrastructure;
using EMS.Modules.Ticketing.Infrastructure;
using EMS.Modules.Users.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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

builder.Services.AddApplication([
    EMS.Modules.Events.Application.AssemblyReference.Assembly,
    EMS.Modules.Users.Application.AssemblyReference.Assembly,
    EMS.Modules.Ticketing.Application.AssemblyReference.Assembly,
    EMS.Modules.Attendance.Application.AssemblyReference.Assembly]);

string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Cache")!;

builder.Services.AddInfrastructure(
    [
        EventsModule.ConfigureConsumers(redisConnectionString),
        TicketingModule.ConfigureConsumers,
        AttendanceModule.ConfigureConsumers,
    ],
    databaseConnectionString,
    redisConnectionString);

builder.Configuration.AddModuleConfiguration(["events", "users", "ticketing", "attendance"]);

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString)
    .AddUrlGroup(new Uri(builder.Configuration.GetValue<string>("KeyCloak:HealthUrl")!), HttpMethod.Get, "keycloak");

builder.Services.AddEventsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddTicketingModule(builder.Configuration);
builder.Services.AddAttendanceModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.MapEndpoints();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.Run();

//Surpass intentionally for testing
#pragma warning disable CA1515 // Consider making public types internal
public partial class Program;
#pragma warning restore CA1515 // Consider making public types internal
