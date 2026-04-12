FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SalonBooking.API/SalonBooking.API.csproj", "SalonBooking.API/"]
COPY ["SalonBooking.CrossCutting/SalonBooking.CrossCutting.csproj", "SalonBooking.CrossCutting/"]
COPY ["SalonBooking.Application/SalonBooking.Application.csproj", "SalonBooking.Application/"]
COPY ["SalonBooking.Infrastructure/SalonBooking.Infrastructure.csproj", "SalonBooking.Infrastructure/"]
COPY ["SalonBooking.Domain/SalonBooking.Domain.csproj", "SalonBooking.Domain/"]
COPY ["Directory.Build.props", "."]

RUN dotnet restore "SalonBooking.API/SalonBooking.API.csproj"

COPY . .
WORKDIR "/src/SalonBooking.API"
RUN dotnet build "SalonBooking.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalonBooking.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalonBooking.API.dll"]
