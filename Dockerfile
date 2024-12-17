FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /App

COPY ./DeleteContact/. ./

RUN dotnet restore  ./DeleteContact/DeleteContact.Api.csproj
RUN dotnet build ./DeleteContact/DeleteContact.Api.csproj
RUN dotnet publish  ./DeleteContact/DeleteContact.Api.csproj -c Release --output Out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /App
COPY --from=build /App/Out/ ./

ENTRYPOINT ["dotnet", "DeleteContact.Api.dll"]

