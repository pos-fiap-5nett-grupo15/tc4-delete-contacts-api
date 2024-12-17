FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /App

COPY ./DeleteContact/. ./

RUN dotnet restore  ./DeleteContract.Worker/DeleteContract.Worker.csproj
RUN dotnet build ./DeleteContract.Worker/DeleteContract.Worker.csproj
RUN dotnet publish  ./DeleteContract.Worker/DeleteContract.Worker.csproj -c Release --output Out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /App
COPY --from=build /App/Out/ ./

ENTRYPOINT ["dotnet", "DeleteContract.Worker.dll"]

