Neblina

# Instruções

`dotnet restore` em cada projeto;

`dotnet ef database update` em cada projeto;

`dotnet run` em cada projeto;

### Para rodar o projeto em uma porta diferente da 5000:

Unix: `ASPNETCORE_URLS="https://*:5123" dotnet run`

Windows PowerShell: `$env:ASPNETCORE_URLS="https://*:5123" ; dotnet run`

Windows CMD (nota: sem aspas): `SET ASPNETCORE_URLS=https://*:5123 && dotnet run`
