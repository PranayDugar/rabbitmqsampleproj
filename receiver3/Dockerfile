# Use the official .NET 5 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files into the container
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET 5 runtime image as the base image for the final stage
FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/out ./

# Set the entry point for the application
ENTRYPOINT ["dotnet", "receiver3.dll"]
