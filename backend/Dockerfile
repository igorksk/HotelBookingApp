FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7263

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HotelBookingApi/HotelBookingApi.csproj", "HotelBookingApi/"]
RUN dotnet restore "HotelBookingApi/HotelBookingApi.csproj"
COPY . .
WORKDIR "/src/HotelBookingApi"
RUN dotnet build "HotelBookingApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelBookingApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelBookingApi.dll"] 