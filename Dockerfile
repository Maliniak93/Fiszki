# 1) Build client and server
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore
COPY Fiszki.sln ./
COPY src/Fiszki/Fiszki/Fiszki.csproj src/Fiszki/Fiszki/
COPY src/Fiszki/Fiszki.Client/Fiszki.Client.csproj src/Fiszki/
RUN dotnet restore "src/Fiszki/Fiszki/Fiszki.csproj"

# Copy all and publish
COPY . .
RUN dotnet publish "src/Fiszki/Fiszki/Fiszki.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2) Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Fiszki.dll"]
