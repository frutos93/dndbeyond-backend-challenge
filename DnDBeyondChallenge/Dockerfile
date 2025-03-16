FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY DnDBeyondChallenge/*.csproj ./DnDBeyondChallenge/
COPY *.sln ./

# Restore dependencies
RUN dotnet restore

# Copy remaining files and build
COPY . . 
WORKDIR /app/DnDBeyondChallenge
RUN dotnet publish -c Release -o /out

# Use a lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Expose API port and run
EXPOSE 5000
CMD ["dotnet", "DnDBeyondChallenge.dll"]