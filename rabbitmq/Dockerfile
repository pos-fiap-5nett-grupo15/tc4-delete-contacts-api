FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /App

COPY ./DeleteContact/. ./

RUN dotnet restore  ./DeleteContact.Worker/DeleteContact.Worker.csproj
RUN dotnet build ./DeleteContact.Worker/DeleteContact.Worker.csproj
RUN dotnet publish  ./DeleteContact.Worker/DeleteContact.Worker.csproj -c Release --output Out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /App
COPY --from=build /App/Out/ ./

ENTRYPOINT ["dotnet", "DeleteContact.Worker.dll"]

