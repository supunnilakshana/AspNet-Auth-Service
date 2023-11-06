#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BS_Web_Api/BS_Web_Api.csproj", "BS_Web_Api/"]
RUN dotnet restore "BS_Web_Api/BS_Web_Api.csproj"
COPY . .
WORKDIR "/src/BS_Web_Api"
RUN dotnet build "BS_Web_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BS_Web_Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BS_Web_Api.dll"]