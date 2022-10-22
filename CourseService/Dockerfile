FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CourseService/CourseService.csproj", "CourseService/"]
RUN dotnet restore "CourseService/CourseService.csproj"
COPY . .
WORKDIR "/src/CourseService"
RUN dotnet build "CourseService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CourseService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseService.dll"]
