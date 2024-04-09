#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 7217
EXPOSE 7216
EXPOSE 2525
EXPOSE 587
EXPOSE 465
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["vogue-decor.API/vogue-decor.API.csproj", "vogue-decor.API/"]
COPY ["vogue-decor.Persistence/vogue-decor.Persistence.csproj", "vogue-decor.Persistence/"]
COPY ["vogue-decor.Application/vogue-decor.Application.csproj", "vogue-decor.Application/"]
COPY ["vogue-decor.Domain/vogue-decor.Domain.csproj", "vogue-decor.Domain/"]
RUN dotnet restore "vogue-decor.API/vogue-decor.API.csproj"
COPY . .
WORKDIR "/src/vogue-decor.API"
RUN dotnet build "vogue-decor.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vogue-decor.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["vogue-decor.API/wwwroot", "wwwroot"]
ENTRYPOINT ["dotnet", "vogue-decor.API.dll"]