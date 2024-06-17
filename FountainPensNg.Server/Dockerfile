#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

#copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

#build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "FountainPensNg.Server.dll" ]


# Stage 1: Build the Angular application
# FROM node:18 AS angular-build
# WORKDIR /app
# COPY ./fountainpensng.client/package*.json ./
# RUN npm install
# COPY ./fountainpensng.client .
# RUN npm run build --prod

# Stage 2: Build the .NET application
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# COPY *.csproj ./
# COPY ["FountainPensNg.Server/FountainPensNg.Server.csproj", "FountainPensNg.Server/"]
# RUN dotnet restore "FountainPensNg.Server\FountainPensNg.Server.csproj"
# COPY . .
# WORKDIR "/src/FountainPensNg.Server"
# RUN dotnet build "FountainPensNg.Server.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "FountainPensNg.Server.csproj" -c Release -o /app/publish

# Stage 3: Combine Angular and .NET build outputs
# FROM base AS final
# WORKDIR /app
# COPY --from=angular-build /app/dist /app/dist
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "FountainPensNg.Server.dll"]