using EMS.Api.Extensions;
using EMS.Common.Application;
using EMS.Common.Infrastructure;
using EMS.Modules.Events.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
});

builder.Services.AddApplication([EMS.Modules.Events.Application.AssemblyReference.Assembly]);
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Database")!);
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

app.Run();
