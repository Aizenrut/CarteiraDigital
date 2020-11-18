#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CarteiraDigital.Api/CarteiraDigital.Api.csproj", "CarteiraDigital.Api/"]
COPY ["CarteiraDigital.Servicos/CarteiraDigital.Servicos.csproj", "CarteiraDigital.Servicos/"]
COPY ["CarteiraDigital.Dados/CarteiraDigital.Dados.csproj", "CarteiraDigital.Dados/"]
COPY ["CarteiraDigital.Models/CarteiraDigital.Models.csproj", "CarteiraDigital.Models/"]
RUN dotnet restore "CarteiraDigital.Api/CarteiraDigital.Api.csproj"
COPY . .
WORKDIR "/src/CarteiraDigital.Api"
RUN dotnet build "CarteiraDigital.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarteiraDigital.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarteiraDigital.Api.dll"]