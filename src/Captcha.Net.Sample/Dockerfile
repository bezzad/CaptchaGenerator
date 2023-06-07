#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
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