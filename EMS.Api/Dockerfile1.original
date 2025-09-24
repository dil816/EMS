# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["EMS.Api/EMS.Api.csproj", "EMS.Api/"]
COPY ["EMS.Modules.Attendance.Infrastructure/EMS.Modules.Attendance.Infrastructure.csproj", "EMS.Modules.Attendance.Infrastructure/"]
COPY ["EMS.Common.Infrastructure/EMS.Common.Infrastructure.csproj", "EMS.Common.Infrastructure/"]
COPY ["EMS.Common.Application/EMS.Common.Application.csproj", "EMS.Common.Application/"]
COPY ["EMS.Common.Domain/EMS.Common.Domain.csproj", "EMS.Common.Domain/"]
COPY ["EMS.Modules.Attendance.Application/EMS.Modules.Attendance.Application.csproj", "EMS.Modules.Attendance.Application/"]
COPY ["EMS.Modules.Attendance.Domain/EMS.Modules.Attendance.Domain.csproj", "EMS.Modules.Attendance.Domain/"]
COPY ["EMS.Modules.Attendance.IntegrationEvents/EMS.Modules.Attendance.IntegrationEvents.csproj", "EMS.Modules.Attendance.IntegrationEvents/"]
COPY ["EMS.Modules.Attendance.Presentation/EMS.Modules.Attendance.Presentation.csproj", "EMS.Modules.Attendance.Presentation/"]
COPY ["EMS.Common.Presentation/EMS.Common.Presentation.csproj", "EMS.Common.Presentation/"]
COPY ["EMS.Modules.Events.IntegrationEvents/EMS.Modules.Events.IntegrationEvents.csproj", "EMS.Modules.Events.IntegrationEvents/"]
COPY ["EMS.Modules.Ticketing.IntegrationEvents/EMS.Modules.Ticketing.IntegrationEvents.csproj", "EMS.Modules.Ticketing.IntegrationEvents/"]
COPY ["EMS.Modules.Users.IntegrationEvents/EMS.Modules.Users.IntegrationEvents.csproj", "EMS.Modules.Users.IntegrationEvents/"]
COPY ["EMS.Modules.Events.Infrastructure/EMS.Modules.Events.Infrastructure.csproj", "EMS.Modules.Events.Infrastructure/"]
COPY ["EMS.Modules.Events.Application/EMS.Modules.Events.Application.csproj", "EMS.Modules.Events.Application/"]
COPY ["EMS.Modules.Events.Domain/EMS.Modules.Events.Domain.csproj", "EMS.Modules.Events.Domain/"]
COPY ["EMS.Modules.Events.Presentation/EMS.Modules.Events.Presentation.csproj", "EMS.Modules.Events.Presentation/"]
COPY ["EMS.Modules.Events.PublicApi/EMS.Modules.Events.PublicApi.csproj", "EMS.Modules.Events.PublicApi/"]
COPY ["EMS.Modules.Users.Infrastructure/EMS.Modules.Users.Infrastructure.csproj", "EMS.Modules.Users.Infrastructure/"]
COPY ["EMS.Modules.Users.Application/EMS.Modules.Users.Application.csproj", "EMS.Modules.Users.Application/"]
COPY ["EMS.Modules.Users.Domain/EMS.Modules.Users.Domain.csproj", "EMS.Modules.Users.Domain/"]
COPY ["EMS.Modules.Users.Presentation/EMS.Modules.Users.Presentation.csproj", "EMS.Modules.Users.Presentation/"]
RUN dotnet restore "./EMS.Api/EMS.Api.csproj"
COPY . .
WORKDIR "/src/EMS.Api"
RUN dotnet build "./EMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EMS.Api.dll"]