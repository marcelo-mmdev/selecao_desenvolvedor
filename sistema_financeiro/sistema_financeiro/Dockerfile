# Etapa 1: build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo para dentro do container
COPY . ./

# Restaura dependências e compila
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa 2: imagem de runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .

# Define o comando de início
ENTRYPOINT ["dotnet", "MeuProjetoCsharp.dll"]
