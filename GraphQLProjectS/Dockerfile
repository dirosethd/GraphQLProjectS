FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["GraphQLProjectS/GraphQLProjectS.csproj", "GraphQLProjectS/"]
RUN dotnet restore "GraphQLProjectS/GraphQLProjectS.csproj"

COPY . .
WORKDIR "/src/GraphQLProjectS"
RUN dotnet publish "GraphQLProjectS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "GraphQLProjectS.dll"]
