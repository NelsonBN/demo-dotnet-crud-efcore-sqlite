FROM mcr.microsoft.com/dotnet/aspnet:8.0.3 AS base-env

WORKDIR /app
EXPOSE 8080

USER app

ENV ASPNETCORE_ENVIRONMENT Production
ENV ASPNETCORE_URLS http://*:8080



FROM mcr.microsoft.com/dotnet/sdk:8.0.203 AS build-env

WORKDIR /src

COPY ./src/*.csproj .

RUN dotnet restore ./*.csproj

COPY ./src/ .

RUN dotnet build -c Release ./*.csproj

RUN dotnet publish ./*.csproj \
    -c Release \
    --no-build \
    --no-restore \
    -o /publish



# Adding a new stage for running migrations
FROM build-env AS migration-env

# Install dotnet ef tool globally if it is not already part of your project
RUN dotnet tool install --global dotnet-ef

# Temporarily setting the PATH to include dotnet tools
ENV PATH="${PATH}:/root/.dotnet/tools"

# Remove existing migrations
RUN dotnet ef migrations remove

# Add a new migration
RUN dotnet ef migrations add Init

# Update the database
RUN dotnet ef database update



FROM base-env AS run-env

WORKDIR /app
COPY --from=build-env /publish .
COPY --from=migration-env /src/bin/app.db ./data/app.db

COPY ./entrypoint.sh /usr/local/bin/

ENTRYPOINT ["entrypoint.sh"]
