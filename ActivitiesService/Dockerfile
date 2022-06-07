FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ActivitiesService/ActivitiesService.csproj", "ActivitiesService/"]
RUN dotnet restore "ActivitiesService/ActivitiesService.csproj"
COPY . .
WORKDIR "/src/ActivitiesService"
RUN dotnet build "ActivitiesService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ActivitiesService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ActivitiesService.dll"]
