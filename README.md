# API Library (api_library_cs)

Guia rápido para clonar, restaurar dependências e executar o projeto (.NET 8, C# 12).

## Requisitos
- .NET SDK 8.x instalado: https://dotnet.microsoft.com/download
- Visual Studio 2022 (opcional, com workload ASP.NET and web development)
- (Opcional) `dotnet-ef` para migrations: `dotnet tool install --global dotnet-ef`

## Clonar o repositório
```bash
https://github.com/TiagoLinharess/api-library-cs.git
```

## Restaurar dependências (CLI .NET)
No diretório do projeto (onde está o `.csproj` ou da solução):	
```bash
dotnet restore && dotnet run --project api-library-cs.csproj
````