FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-nodejs-v20 AS builder

WORKDIR /src

COPY NuGet.Config .

COPY src/Fanex.Athena.BetTicket.Core/Fanex.Athena.BetTicket.Core.csproj ./Fanex.Athena.BetTicket.Core/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Fanex.Athena.BetTicket.Core/Fanex.Athena.BetTicket.Core.csproj -v n

COPY src/Modules/Fanex.Athena.BetTicket.Detail/Fanex.Athena.BetTicket.Detail.csproj ./Modules/Fanex.Athena.BetTicket.Detail/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Modules/Fanex.Athena.BetTicket.Detail/Fanex.Athena.BetTicket.Detail.csproj -v n

COPY src/Modules/Fanex.Athena.BetTicket.Result/Fanex.Athena.BetTicket.Result.csproj ./Modules/Fanex.Athena.BetTicket.Result/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Modules/Fanex.Athena.BetTicket.Result/Fanex.Athena.BetTicket.Result.csproj -v n

COPY src/Fanex.Athena.BetTicket.WebHost/Fanex.Athena.BetTicket.WebHost.csproj ./Fanex.Athena.BetTicket.WebHost/
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore Fanex.Athena.BetTicket.WebHost/Fanex.Athena.BetTicket.WebHost.csproj -v n

COPY src .

WORKDIR /src/Fanex.Athena.BetTicket.WebHost

COPY src/Fanex.Athena.BetTicket.WebHost/package.json ./
COPY src/Fanex.Athena.BetTicket.WebHost/package-lock.json ./

ARG TRANSFORM_CONFIG

RUN node -v
RUN npm -v
RUN npm install

# Build front-end
RUN case "$TRANSFORM_CONFIG" in \
    (LOCAL|SIT) npm run build-dev;; \
    (*) npm run build-pro;; \
    esac
#

RUN dotnet publish "Fanex.Athena.BetTicket.WebHost.csproj" -c Release -o /publish --self-contained false

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1

# Dont run as root
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000

WORKDIR /app

COPY --from=builder /publish .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.Athena.BetTicket.WebHost.dll"]