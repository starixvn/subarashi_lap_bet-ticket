ARG CI_REGISTRY_IMAGE

FROM $CI_REGISTRY_IMAGE:builder as deps

RUN mkdir -p /src/packages

FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine-nodejs AS builder

ENV NUGET_PACKAGES=/src/packages

COPY --from=deps $NUGET_PACKAGES/. $NUGET_PACKAGES/

WORKDIR /src/Fanex.Athena.BetTicket.WebHost

COPY src/Fanex.Athena.BetTicket.WebHost/package.json ./
COPY src/Fanex.Athena.BetTicket.WebHost/package-lock.json ./

RUN node -v
RUN npm -v
RUN npm install

WORKDIR /src

COPY NuGet.Config .

COPY src/Fanex.Athena.BetTicket.Core/Fanex.Athena.BetTicket.Core.csproj ./Fanex.Athena.BetTicket.Core/
RUN dotnet restore Fanex.Athena.BetTicket.Core/Fanex.Athena.BetTicket.Core.csproj -v n

COPY src/Modules/Fanex.Athena.BetTicket.Detail/Fanex.Athena.BetTicket.Detail.csproj ./Modules/Fanex.Athena.BetTicket.Detail/
RUN dotnet restore Modules/Fanex.Athena.BetTicket.Detail/Fanex.Athena.BetTicket.Detail.csproj -v n

COPY src/Modules/Fanex.Athena.BetTicket.Result/Fanex.Athena.BetTicket.Result.csproj ./Modules/Fanex.Athena.BetTicket.Result/
RUN dotnet restore Modules/Fanex.Athena.BetTicket.Result/Fanex.Athena.BetTicket.Result.csproj -v n

COPY src/Fanex.Athena.BetTicket.WebHost/Fanex.Athena.BetTicket.WebHost.csproj ./Fanex.Athena.BetTicket.WebHost/
RUN dotnet restore Fanex.Athena.BetTicket.WebHost/Fanex.Athena.BetTicket.WebHost.csproj -v n

COPY src .

WORKDIR /src/Fanex.Athena.BetTicket.WebHost

RUN npm run build-pro

RUN dotnet publish "Fanex.Athena.BetTicket.WebHost.csproj" -c Release -o /publish \
  --runtime linux-x64 \
  --self-contained false

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine

ENV ASPNETCORE_URLS=http://*:5000
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
EXPOSE 5000

WORKDIR /app

COPY --from=builder /publish .

ENTRYPOINT ["dotnet", "Fanex.Athena.BetTicket.WebHost.dll"]