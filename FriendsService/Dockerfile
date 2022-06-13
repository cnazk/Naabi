FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FriendsService/FriendsService.csproj", "FriendsService/"]
RUN dotnet restore "FriendsService/FriendsService.csproj"
COPY . .
WORKDIR "/src/FriendsService"
RUN dotnet build "FriendsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FriendsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FriendsService.dll"]
