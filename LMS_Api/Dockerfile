#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LMS_Api/LMS_Api.csproj", "LMS_Api/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Utils/Utils.csproj", "Utils/"]
RUN dotnet restore "LMS_Api/LMS_Api.csproj"
COPY . .
WORKDIR "/src/LMS_Api"
RUN dotnet build "LMS_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LMS_Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LMS_Api.dll"]
