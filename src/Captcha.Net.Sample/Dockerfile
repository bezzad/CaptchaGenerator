#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM docker.mofid.dev/mofidonline/dotnet/aspnet-webapi-libgdplus:6.0
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

# Install the fontconfig package
RUN apt-get update && \
    apt-get install -y fontconfig
    
# Copy the font files into the container
COPY --from=build /app/fonts/*.ttf /usr/share/fonts/truetype/

# Update the font cache
RUN fc-cache -f -v

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Captcha.Net.Sample.dll"]