FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CarteiraDigital.ProvedorAutenticacao/CarteiraDigital.ProvedorAutenticacao.csproj", "CarteiraDigital.ProvedorAutenticacao/"]
COPY ["CarteiraDigital.Models/CarteiraDigital.Models.csproj", "CarteiraDigital.Models/"]
COPY ["CarteiraDigital.Servicos/CarteiraDigital.Servicos.csproj", "CarteiraDigital.Servicos/"]
COPY ["CarteiraDigital.Dados/CarteiraDigital.Dados.csproj", "CarteiraDigital.Dados/"]
RUN dotnet restore "CarteiraDigital.ProvedorAutenticacao/CarteiraDigital.ProvedorAutenticacao.csproj"
COPY . .
WORKDIR "/src/CarteiraDigital.ProvedorAutenticacao"
RUN dotnet build "CarteiraDigital.ProvedorAutenticacao.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarteiraDigital.ProvedorAutenticacao.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarteiraDigital.ProvedorAutenticacao.dll"]