#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["SongtrackerPro.Api/SongtrackerPro.Api.csproj", "SongtrackerPro.Api/"]
COPY ["SongtrackerPro.Data/SongtrackerPro.Data.csproj", "SongtrackerPro.Data/"]
COPY ["SongtrackerPro.Resources/SongtrackerPro.Resources.csproj", "SongtrackerPro.Resources/"]
COPY ["SongtrackerPro.Utilities/SongtrackerPro.Utilities.csproj", "SongtrackerPro.Utilities/"]
COPY ["SongtrackerPro.Tasks/SongtrackerPro.Tasks.csproj", "SongtrackerPro.Tasks/"]
RUN dotnet restore "SongtrackerPro.Api/SongtrackerPro.Api.csproj"
COPY . .
WORKDIR "/src/SongtrackerPro.Api"
RUN dotnet build "SongtrackerPro.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SongtrackerPro.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SongtrackerPro.Api.dll"]
