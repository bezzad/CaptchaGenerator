# FROM docker.mofid.dev/mofidonline/dotnet/aspnet-webapi-libgdplus:6.0
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base

# Install the fontconfig package
RUN apt-get update; apt-get install -y fontconfig fonts-liberation

# Update the font cache
RUN fc-cache -f -v

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["Captcha.Net.Sample/Captcha.Net.Sample.csproj", "Captcha.Net.Sample/"]
COPY ["Captcha.Net/Captcha.Net.csproj", "Captcha.Net/"]
RUN dotnet restore "Captcha.Net.Sample/Captcha.Net.Sample.csproj"
COPY . .
WORKDIR "/src/Captcha.Net.Sample"
RUN dotnet build "Captcha.Net.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Captcha.Net.Sample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Captcha.Net.Sample.dll"]