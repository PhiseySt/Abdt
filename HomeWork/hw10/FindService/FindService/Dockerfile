FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ./FindService ./FindService
COPY ./AuthenticationBase ./AuthenticationBase
COPY ./RepositoryBase ./RepositoryBase
COPY ./TextService/TextService.Repositories ./TextService/TextService.Repositories
COPY ./TextService/TextService.Services ./TextService/TextService.Services
COPY ./TextService/TextService.Entities ./TextService/TextService.Entities
COPY ./TextService/TextService.Client ./TextService/TextService.Client


WORKDIR ./FindService/FindService

RUN dotnet restore "FindService.csproj"
RUN dotnet build "FindService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FindService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FindService.dll"]