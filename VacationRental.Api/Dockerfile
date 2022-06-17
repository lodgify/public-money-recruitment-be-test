#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VacationRental.Api/VacationRental.Api.Host.csproj", "VacationRental.Api/"]
COPY ["VacationRental.Models/VacationRental.Models.csproj", "VacationRental.Models/"]
COPY ["VacationRental.Services/VacationRental.Services.csproj", "VacationRental.Services/"]
COPY ["VacationRental.DataAccess.Models/VacationRental.DataAccess.Models.csproj", "VacationRental.DataAccess.Models/"]
COPY ["VacationRental.DataAccess/VacationRental.DataAccess.csproj", "VacationRental.DataAccess/"]
RUN dotnet restore "VacationRental.Api/VacationRental.Api.Host.csproj"
COPY . .
WORKDIR "/src/VacationRental.Api"
RUN dotnet build "VacationRental.Api.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VacationRental.Api.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VacationRental.Api.Host.dll"]