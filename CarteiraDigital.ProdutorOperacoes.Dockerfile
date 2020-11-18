#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CarteiraDigital.ProdutorOperacoes/CarteiraDigital.ProdutorOperacoes.csproj", "CarteiraDigital.ProdutorOperacoes/"]
COPY ["CarteiraDigital.Models/CarteiraDigital.Models.csproj", "CarteiraDigital.Models/"]
RUN dotnet restore "CarteiraDigital.ProdutorOperacoes/CarteiraDigital.ProdutorOperacoes.csproj"
COPY . .
WORKDIR "/src/CarteiraDigital.ProdutorOperacoes"
RUN dotnet build "CarteiraDigital.ProdutorOperacoes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarteiraDigital.ProdutorOperacoes.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarteiraDigital.ProdutorOperacoes.dll"]