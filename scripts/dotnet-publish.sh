cd ../src
dotnet publish --force ./Efforteo.Services.Activities -c Debug -o ./bin/Docker
dotnet publish --force ./Efforteo.Api -c Debug -o ./bin/Docker
dotnet publish --force ./Efforteo.Services.Identity -c Debug -o ./bin/Docker