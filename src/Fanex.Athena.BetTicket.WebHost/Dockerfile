FROM registry.nexdev.net:5050/docker/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder

RUN apt-get update  \
	&& curl --silent --location https://deb.nodesource.com/setup_14.x | bash -  \
	&& apt-get install -y nodejs

RUN node -v
RUN npm -v

WORKDIR /src/Fanex.Athena.BetTicket.WebHost

COPY src/Fanex.Athena.BetTicket.WebHost/package.json ./
COPY src/Fanex.Athena.BetTicket.WebHost/package-lock.json ./

RUN npm install

WORKDIR /src

COPY NuGet.Config .

COPY src/Fanex.Athena.BetTicket.Infrastructure/Fanex.Athena.BetTicket.Infrastructure.csproj ./Fanex.Athena.BetTicket.Infrastructure/
RUN dotnet restore Fanex.Athena.BetTicket.Infrastructure/Fanex.Athena.BetTicket.Infrastructure.csproj -v n

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
  --runtime alpine-x64 \
  --self-contained false

FROM registry.nexdev.net:5050/docker/dotnet/core/aspnet:5.0-buster-slim

ENV ASPNETCORE_URLS=http://*:5000
EXPOSE 5000

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

WORKDIR /app

COPY --from=builder /publish .

ENTRYPOINT ["dotnet", "Fanex.Athena.BetTicket.WebHost.dll"]