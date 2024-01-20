# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file to the working directory
COPY Rabbitmqdemo.csproj .

# Restore dependencies
RUN dotnet restore Rabbitmqdemo.csproj

# Copy the remaining source code to the working directory
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Use a smaller runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:5.0



# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/out .

EXPOSE 5672

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Rabbitmqdemo.dll"]


# EXPOSE map[15671/tcp:{} 15672/tcp:{}]