# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore as distinct layers
COPY *.sln ./
COPY PresentationLayer/PresentationLayer.csproj ./PresentationLayer/
COPY BusinessLogicLayer/BusinessLogicLayer.csproj ./BusinessLogicLayer/
COPY DataAccessLayer/DataAccessLayer.csproj ./DataAccessLayer/

RUN dotnet restore

# Copy all source files
COPY . .

# Build the application
RUN dotnet publish PresentationLayer/PresentationLayer.csproj -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (change if your app uses another port)
EXPOSE 80

# Entry point
ENTRYPOINT ["dotnet", "PresentationLayer.dll"]
