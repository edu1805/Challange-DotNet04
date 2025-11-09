# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos da solução
COPY ChallangeMottu.sln ./

# Copiar todos os projetos (necessário para o restore funcionar)
COPY ChallangeMottu.Api/ChallangeMottu.Api.csproj ChallangeMottu.Api/
COPY ChallangeMottu.Application/ChallangeMottu.Application.csproj ChallangeMottu.Application/
COPY ChallangeMottu.Domain/ChallangeMottu.Domain.csproj ChallangeMottu.Domain/
COPY ChallangeMottu.Infrastructure/ChallangeMottu.Infrastructure.csproj ChallangeMottu.Infrastructure/

# Restaurar dependências
RUN dotnet restore

# Copiar todo o código
COPY . .

# Publicar a API
WORKDIR /src/ChallangeMottu.Api
RUN dotnet publish -c Release -o /app/publish \
    /p:GenerateDocumentationFile=true \
    /p:DocumentationFile=/app/publish/ChallangeMottu.Api.xml

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar publicação
COPY --from=build /app/publish .

# Configurações da API
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

# Executar a API
CMD ["dotnet", "ChallangeMottu.Api.dll"]
