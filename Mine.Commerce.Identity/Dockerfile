FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

COPY ["Mine.Commerce.Identity/Mine.Commerce.Identity.csproj", "./"]
RUN dotnet restore "./Mine.Commerce.Identity/Mine.Commerce.Identity.csproj"
COPY ./Mine.Commerce.Identity .
RUN dotnet build "Mine.Commerce.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mine.Commerce.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mine.Commerce.Identity.dll"]
