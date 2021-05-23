#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ./TaskService ./TaskService
COPY ./AuthenticationBase ./AuthenticationBase
COPY ./RepositoryBase ./RepositoryBase

COPY ./FindService/FindService.Services ./FindService/FindService.Services
COPY ./FindService/FindService.Client ./FindService/FindService.Client

COPY ./TextService/TextService.Repositories ./TextService/TextService.Repositories
COPY ./TextService/TextService.Services ./TextService/TextService.Services
COPY ./TextService/TextService.Entities ./TextService/TextService.Entities
COPY ./TextService/TextService.Client ./TextService/TextService.Client

WORKDIR ./TaskService/TaskService
RUN dotnet restore "TaskService.csproj"
RUN dotnet build "TaskService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskService.dll"]